using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMSharpConsoleTest
{
    class ScalarEncoder:BaseEncoder
    {
        int N;
        int W;
        int HalfWidth;
        double MinValue;
        double MaxValue;
        double RangeInternal;

        int NInternal;
        double Range;
        int Padding;
        double Resolution;
        double Radius;

        bool IsPeriodic;
        bool ClipInput;


        public ScalarEncoder(int w, double minvalue, double maxvalue, bool periodic, 
            int n, int radius, int resolution, 
            bool clipInput)
        {
            if (w > 0)
                W = w;
            else
                throw new Exception("ScalarEncoder(): w must be > 0");

            if (double.IsNaN(minvalue))
                throw new Exception("minvalue is NaN");
            if (double.IsNaN(maxvalue))
                throw new Exception("maxvalue is NaN");

            if (minvalue >= maxvalue)
                throw new Exception("minvalue ("+minvalue+") must be less than maxvalue ("+maxvalue+")");

            MinValue = minvalue;
            MaxValue = maxvalue;

            IsPeriodic = periodic;
            ClipInput = clipInput;

            HalfWidth = (w - 1) / 2;

            Padding = IsPeriodic ? 0 : HalfWidth;

            RangeInternal = MaxValue - MinValue;

            InitEncoder(w, minvalue, maxvalue, n, radius, resolution);

            NInternal = N - 2 * Padding;
        }

        private void InitEncoder(int w, double minvalue, double maxvalue, int n, double radius, double resolution)
        {
            if (n != 0)
            {
                if (n <= w)
                    throw new Exception("InitEncoder() (scalar) exception: n (" + n + ") must be > w (" + w + ")");

                this.N = n;

                if (double.IsNaN(minvalue))
                    throw new Exception("minvalue is NaN");
                if (double.IsNaN(maxvalue))
                    throw new Exception("maxvalue is NaN");

                if (minvalue >= maxvalue)
                    throw new Exception("minvalue (" + minvalue + ") must be less than maxvalue (" + maxvalue + ")");

                this.Resolution = this.IsPeriodic ? (RangeInternal / N) : (RangeInternal / (N - W));
                this.Radius = W * Resolution;
                this.Range = this.IsPeriodic ? RangeInternal : RangeInternal + Resolution;
            }
            else
            {
                if (radius != 0)
                {
                    this.Radius = radius;
                    this.Resolution = Radius / W;
                }
                else if (resolution != 0)
                {
                    this.Resolution = resolution;
                    this.Radius = Resolution * W;
                }
                else
                {
                    throw new Exception("InitEncoder() (scalar) exception: n, radius or resolution must be specified");
                }

                if (double.IsNaN(minvalue))
                    throw new Exception("minvalue is NaN");
                if (double.IsNaN(maxvalue))
                    throw new Exception("maxvalue is NaN");

                if (minvalue >= maxvalue)
                    throw new Exception("minvalue (" + minvalue + ") must be less than maxvalue (" + maxvalue + ")");

                this.Range = this.IsPeriodic ? RangeInternal : RangeInternal + Resolution;

                var nfloat = W * (Range / Radius) + 2 * Padding;
                this.N = (int)Math.Ceiling(nfloat);
            }
        }

        public BitArray Encode(double input)
        {
            if (double.IsNaN(input))
                return new BitArray(N);

            int? bucketIndx = GetFirstOnBit(input);
            if (bucketIndx != null)
            {

            }
            return null;
        }

        public void Encode(double input, BitArray sdrArray)
        {
            if (double.IsNaN(input))
                return;
            if (sdrArray == null) sdrArray = new BitArray(N);
            else
                sdrArray.SetAll(false);

            int? bucketIndx = GetFirstOnBit(input);
            if (bucketIndx != null)
            {
                var min_bin = bucketIndx.Value;
                var max_bin = min_bin + 2 * HalfWidth;
                
                if (IsPeriodic)
                {
                    // Handle the edges by computing wrap-around
                    if (max_bin >= N)
                    {
                        var bottombins = max_bin - N + 1;
                        var bit_range = MakeRange(0, bottombins);
                        SetIndexes(sdrArray, bit_range, true);
                        max_bin = N - 1;
                    }

                    if (min_bin < 0)
                    {
                        var topbins = -min_bin;
                        var bit_range = MakeRange(N-topbins, N);
                        SetIndexes(sdrArray, bit_range, true);
                        min_bin = 0;
                    }
                }

                SetIndexes(sdrArray, MakeRange(min_bin, max_bin + 1), true);
            }
        }

        public int? GetFirstOnBit(double input)
        {
            if (double.IsNaN(input))
                return null;

            //Clip Min
            if (input < MinValue)
            {
                if (ClipInput && IsPeriodic)
                {
                    input = MinValue;
                }
                else
                    throw new InvalidOperationException("input (" + input + ") less than range (" +
                           MinValue + " - " + MaxValue);
            }

            if (IsPeriodic)
            {
                if (input >= MaxValue)
                    throw new InvalidOperationException("input (" + input + ") greater than periodic range (" +
                       MinValue + " - " + MaxValue);
            }
            else
            {
                if (input > MaxValue)
                {
                    if (ClipInput)
                    {
                        input = MaxValue;
                    }
                    else
                        throw new InvalidOperationException("input (" + input + ") greater than periodic range (" +
                           MinValue + " - " + MaxValue);
                }

            }

            int center;
            if (IsPeriodic)
            {
                center = ((int)((input - MinValue) * NInternal / Range)) + Padding;
            }
            else
            {
                center = (int)(((input - MinValue) * Resolution / 2) / Resolution) + Padding;
            }

            var minbin = center - HalfWidth;
            return minbin;
        }

        public void SetIndexes(BitArray bitArray, int[] indexes, bool valueToSet)
        {
            if (indexes != null && bitArray != null)
            {
                foreach (var index in indexes)
                {
                    if (index >= 0)
                        bitArray[index] = valueToSet;
                }
            }
        }

        
    }
}
