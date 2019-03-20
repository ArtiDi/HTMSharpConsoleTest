using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMSharpConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// Analog to numpy.unravel_index
        /// </summary>
        /// <param name="index">Plain index of item</param>
        /// <param name="dimensions">Dimension of matrix</param>
        /// <returns></returns>
        static int[] UnravelIndex(int index, int[] dimensions)
        {
            double buf = index;
            var decl_dimensions = dimensions;
            Array.Reverse(decl_dimensions);
            var real_coordinates = new int[decl_dimensions.Length];
            for (int dimensionIndex = 0; dimensionIndex < decl_dimensions.Length; dimensionIndex++)
            {
                real_coordinates[dimensionIndex] = (int)buf % decl_dimensions[dimensionIndex];
                buf = Math.Truncate((buf / decl_dimensions[dimensionIndex]));
                if (buf == 0)
                    break;
            }
            Array.Reverse(real_coordinates);
            return real_coordinates;
        }


        static void IncrementColumnCoordianteRecursive(uint dimension_index, int[] declarative_dimensions, int[] output_coordinates)
        {
            if (declarative_dimensions.Length != output_coordinates.Length) throw new Exception($"Dimension length ({declarative_dimensions.Length}) and output matrix length ({output_coordinates.Length}) do not mutch");

            if (dimension_index >= declarative_dimensions.Length) return;

            output_coordinates[dimension_index]++;
            if (output_coordinates[dimension_index] >= declarative_dimensions[dimension_index])
            {
                output_coordinates[dimension_index] = 0;
                IncrementColumnCoordianteRecursive(dimension_index + 1, declarative_dimensions, output_coordinates);
            }
        }
        static void TestScalarEncoder()
        {
            var encoder = new ScalarEncoder(3, 0, 100, false, 0, 1, 0, true);
            var sdr = new BitArray(50);
            encoder.Encode(23, sdr);

            var repres = BitArrayToString(sdr);
        }

        static void TestRandomScalarEncoder()
        {
            var encoder = new RandomDistributedScalarEncoder(1, 21, 400,1, randomSeed: 3);
            var sdrList = new List<string>();
            var uniqueSDR = new Dictionary<string, List<double>>();

            BitArray sdr = new BitArray(400);
            for (double i = -1000; i < 1001; i+=0.5)
            {
                encoder.Encode(i, sdr);
                var sdr_string = BitArrayToString(sdr);

                if (!uniqueSDR.ContainsKey(sdr_string))
                    uniqueSDR.Add(sdr_string, new List<double>());

                uniqueSDR[sdr_string].Add(i);

            }

        }
        static string BitArrayToString(BitArray array)
        {
            if (array == null) return "";

            string res = "";
            for (int i = 0; i< array.Length; i++)
            {
                var bit = array.Get(i);
                if (bit)
                    res += "1";
                else
                    res += "0";
            }
            return res;
        }
    }
}
