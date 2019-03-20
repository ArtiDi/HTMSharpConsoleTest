using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMSharpConsoleTest
{
    class RandomDistributedScalarEncoder : BaseEncoder
    {
        public int w;
        public int n;
        public double resolution;
        //The largest overlap we allow for non-adjacent encodings
        public int MaxOverlap = 2;
        public int RandomSeed;

        public int MinIndex;
        public int MaxIndex;
        public double Offset;
        public Random random;
        public int NumRetry;

        public const int INITIAL_BUCKETS = 1000;

        ConcurrentDictionary<int, int[]> bucketMap;

        private int MaxBuckets;
        public RandomDistributedScalarEncoder(double resolution, int w = 21, int n = 400, double offset = double.NaN, int randomSeed = 42)
        {
            if ((w <= 0) || (w % 2 == 0))
                throw new Exception("w must be an odd positive integer");

            if (resolution <= 0)
                throw new Exception("resolution must be a positive number");

            if (n <= 6 * w)
                throw new Exception("n must be an int strictly greater than 6*w. For" +
                    "good results we recommend n be strictly greater " +
                       "than 11*w");
            this.w = w;
            this.n = n;
            this.resolution = resolution;
            //initialize the random number generators
            this.RandomSeed = randomSeed;

            InitializeBucketMap(INITIAL_BUCKETS, offset);
        }

        private void InitializeBucketMap(int maxBuckets, double offset)
        {
            MaxBuckets = maxBuckets;
            MinIndex = MaxIndex = MaxBuckets / 2;
            Offset = offset;

            bucketMap = new ConcurrentDictionary<int, int[]>();
            random = RandomSeed == -1 ? new Random() : new Random(RandomSeed);
            var shuffled = MakeRange(0, n).OrderBy(x => random.Next()).ToArray();
            var first = new int[w];
            Array.Copy(shuffled, first, w);
            bucketMap.TryAdd(MinIndex, first);
            NumRetry = 0;
        }


        public void Encode(double input, BitArray output)
        {
            //Check

            //BucketIndices
            var bucketIdx = GetBucketIndices(input);
            output.SetAll(false);

            var indices = MapBucketIndexToNonZeroBits(bucketIdx[0]);

            //Fill
            foreach (int index in indices)
                output[index] = true;
        }

        public int[] GetBucketIndices(double input)
        {
            if (double.IsNaN(input))
                return new int[0];

            if (double.IsNaN(Offset))
                Offset = input;

            /*
             * COPYPAST from htm.java & HTM.Net
             * 
            * Difference in the round function behavior for Python and Java In
            * Python, the absolute value is rounded up and sign is applied in Java,
            * value is rounded to next biggest int
            *
            * so for Python, round(-0.5) => -1.0 whereas in Java, Math.round(-0.5)
            * => 0.0
            */
            double deltaIndex = (input - Offset) / resolution;
            int sign = (int)(deltaIndex / Math.Abs(deltaIndex));
            int bucketIdx = (MaxBuckets / 2)
                    + (sign * (int)Math.Round(Math.Abs(deltaIndex)));

            if (bucketIdx < 0)
                bucketIdx = 0;
            else if (bucketIdx >= MaxBuckets)
                bucketIdx = MaxBuckets - 1;

            int[] bucketIdxArray = new int[1];
            bucketIdxArray[0] = bucketIdx;
            return bucketIdxArray;
        }

        public int[] MapBucketIndexToNonZeroBits(int BucketIndex)
        {
            if (BucketIndex < 0)
                BucketIndex = 0;

            if (BucketIndex >= MaxBuckets)
                BucketIndex = MaxBuckets - 1;

            if (!bucketMap.ContainsKey(BucketIndex))
            {
                CreateBucket(BucketIndex);
            }
            return bucketMap[BucketIndex];
        }

        public void CreateBucket(int BucketIndex)
        {
            if (BucketIndex < MinIndex)
            {
                if (BucketIndex == MinIndex - 1)
                {
                    bucketMap.TryAdd(BucketIndex, NewRepresentation(MinIndex, BucketIndex));
                    MinIndex = BucketIndex;
                }
                else
                {
                    CreateBucket(BucketIndex + 1);
                    CreateBucket(BucketIndex);
                }
            }
            else
            {
                if (BucketIndex == MaxIndex + 1)
                {
                    bucketMap.TryAdd(BucketIndex, NewRepresentation(MaxIndex, BucketIndex));
                    MaxIndex = BucketIndex;
                }
                else
                {
                    CreateBucket(BucketIndex - 1);
                    CreateBucket(BucketIndex);
                }
            }
        }

        public int[] NewRepresentation(int index, int newIndex)
        {
            var prev = bucketMap[index];
            var newRepresentation = new int[prev.Length];
            Array.Copy(prev, newRepresentation, prev.Length);

            var randomIndex = newIndex % w;

            var newBit = random.Next(n);
            newRepresentation[randomIndex] = newBit;
            while (bucketMap[index].Contains(newBit)
                   || !isNewRepresentationOk(newRepresentation, newIndex))
            {
                NumRetry += 1;
                newBit = random.Next(n);
                newRepresentation[randomIndex] = newBit;
            }

            return newRepresentation;
        }

        private bool isNewRepresentationOk(int[] newRepresentation, int newIndex)
        {
            /*
                Return True if this new candidate representation satisfies all our overlap
                rules. Since we know that neighboring representations differ by at most
                one bit, we compute running overlaps.
             */

            if (newRepresentation.Length != w)
                return false;
            if (newIndex < MinIndex - 1 || newIndex > MaxIndex + 1)
            {
                throw new InvalidOperationException(
                        "newIndex must be within one of existing indices");
            }

            // A binary representation of newRep. We will use this to test
            // containment
            var newRepBinary = new BitArray(n);
            newRepBinary.SetAll(false);
            

            foreach (int index in newRepresentation)
                newRepBinary[index] = true;

            // Midpoint
            int midIdx = MaxBuckets / 2;

            // Start by checking the overlap at minIndex
            int runningOverlap = CountOverlap(bucketMap[MinIndex], newRepresentation);
            if (!isOverlapOK(MinIndex, newIndex, runningOverlap))
                return false;

            // Compute running overlaps all the way to the midpoint
            for (int i = MinIndex + 1; i < midIdx + 1; i++)
            {
                // This is the bit that is going to change
                int newBit = (i - 1) % w;

                // Update our running overlap
                if (newRepBinary[bucketMap[i - 1][newBit]])
                    runningOverlap--;
                if (newRepBinary[bucketMap[i][newBit]])
                    runningOverlap++;

                // Verify our rules
                if (!isOverlapOK(i, newIndex, runningOverlap))
                    return false;
            }

            // At this point, runningOverlap contains the overlap for midIdx
            // Compute running overlaps all the way to maxIndex
            for (int i = midIdx + 1; i <= MaxIndex; i++)
            {
                int newBit = i % w;

                // Update our running overlap
                if (newRepBinary[bucketMap[i - 1][newBit]])
                    runningOverlap--;
                if (newRepBinary[bucketMap[i][newBit]])
                    runningOverlap++;

                // Verify our rules
                if (!isOverlapOK(i, newIndex, runningOverlap))
                    return false;
            }

            return true;
        }

        /// <summary>
        ///Return true if the given overlap between bucket indices BucketIndex_1 and BucketIndex_2 are acceptable. If overlap is not specified, calculate it from the bucketMap
        /// </summary>
        /// <param name="BucketIndex_1"></param>
        /// <param name="BucketIndex_2"></param>
        /// <param name="overlap"></param>
        /// <returns></returns>
        public bool isOverlapOK(int BucketIndex_1, int BucketIndex_2, int overlap)
        {
            if (Math.Abs(BucketIndex_1 - BucketIndex_2) < w
                && 
                overlap == w - Math.Abs(BucketIndex_1 - BucketIndex_2))
                return true;

            if (Math.Abs(BucketIndex_1 - BucketIndex_2) >= w
                &&
                overlap <= MaxOverlap)
                return true;
            
            return false;
        }
        public bool isOverlapOK(int i, int j)
        {
            return isOverlapOK(i, j, CountOverlapIndices(i,j));
        }

        private int CountOverlapIndices(int i, int j)
        {
            if (!bucketMap.ContainsKey(i) && !bucketMap.ContainsKey(j))
                throw new Exception(($"bucketMap doesn't contain indices: {i},{j}"));
            else if (!bucketMap.ContainsKey(i))
                throw new Exception($"bucketMap doesn't contain index: {i}");
            else if(!bucketMap.ContainsKey(j))
                throw new Exception($"bucketMap doesn't contain index: {j}");

            var iR = bucketMap[i];
            var jR = bucketMap[j];

            return CountOverlap(iR, jR);
        }

        public static int CountOverlap(int[] a1, int[] a2)
        {
            return a1.Where(x => a2.Contains(x)).Count();
        }
    }
}
