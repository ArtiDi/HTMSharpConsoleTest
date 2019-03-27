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
