using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMSharpConsoleTest.Utils
{
    class ArrayUtils
    {
        #region ELEMENTS PRODUCT
        public static int ElementsProduct(int[] array)
        {
            int prod = 1;

            if (array != null)
                prod = array.AsParallel().Aggregate((x, y) => x * y);

            return prod;
        }
        public static uint ElementsProduct(uint[] array)
        {
            uint prod = 1;

            if (array != null)
                prod = array.AsParallel().Aggregate((x, y) => x * y);

            return prod;
        }
        public static double ElementsProduct(double[] array)
        {
            double prod = 1;

            if (array != null)
                prod = array.AsParallel().Aggregate((x, y) => x * y);

            return prod;
        }
        public static double ElementsProduct(float[] array)
        {
            double prod = 1;

            if (array != null)
                prod = array.AsParallel().Aggregate((x, y) => x * y);

            return prod;
        }
        #endregion ELEMENTS PRODUCT

        #region DIVISION
        public static double[] ItemByItemDivision(uint[] toDivide, uint[] divideBy)
        {
            if (toDivide == null) throw new Exception("'toDivide' is null");
            if (divideBy == null) throw new Exception("'divideBy' is null");
            if (toDivide.Length != divideBy.Length) throw new Exception($"'toDivide' array length ({toDivide.Length}) must be equal to 'divideBy' array length ({divideBy.Length})");

            var resultArray = new double[toDivide.Length];
            for (int i = 0; i < resultArray.Length; i++)
            {
                resultArray[i] = (divideBy[i] == 0 ? toDivide[i] : toDivide[i] / (double)divideBy[i]);
            }
            return resultArray;
        }
        public static double[] ItemByItemDivision(int[] toDivide, int[] divideBy)
        {
            if (toDivide == null) throw new Exception("'toDivide' is null");
            if (divideBy == null) throw new Exception("'divideBy' is null");
            if (toDivide.Length != divideBy.Length) throw new Exception($"'toDivide' array length ({toDivide.Length}) must be equal to 'divideBy' array length ({divideBy.Length})");

            var resultArray = new double[toDivide.Length];
            for (int i = 0; i < resultArray.Length; i++)
            {
                resultArray[i] = (divideBy[i] == 0 ?
                    toDivide[i]
                    :
                    toDivide[i] / (double)divideBy[i]);
            }
            return resultArray;
        }
        #endregion DIVISION

        #region MULTIPLICATION
        // ITEM BY ITEM
        public static double[] ItemByItemMultiply(uint[] array1, double[] array2)
        {
            if (array1 == null) throw new Exception("'array1' is null");
            if (array2 == null) throw new Exception("'array2' is null");
            if (array1.Length != array1.Length) throw new Exception($"'array1' length ({array1.Length}) must be equal to 'array2' length ({array2.Length})");

            var result = new double[array1.Length];
            for (int i = 0; i < array1.Length; i++)
            {
                result[i] = array1[i] * array2[i];
            }
            return result;
        }
        public static uint[] ItemByItemMultiply(uint[] array1, uint[] array2)
        {
            if (array1 == null) throw new Exception("'array1' is null");
            if (array2 == null) throw new Exception("'array2' is null");
            if (array1.Length != array1.Length) throw new Exception($"'array1' length ({array1.Length}) must be equal to 'array2' length ({array2.Length})");

            var result = new uint[array1.Length];
            for (int i = 0; i < array1.Length; i++)
            {
                result[i] = array1[i] * array2[i];
            }
            return result;
        }
        public static uint[] ItemByItemMultiply(uint[] array1, int[] array2)
        {
            if (array1 == null) throw new Exception("'array1' is null");
            if (array2 == null) throw new Exception("'array2' is null");
            if (array1.Length != array1.Length) throw new Exception($"'array1' length ({array1.Length}) must be equal to 'array2' length ({array2.Length})");

            var result = new uint[array1.Length];
            for (int i = 0; i < array1.Length; i++)
            {
                result[i] = (uint)(array1[i] * array2[i]);
            }
            return result;
        }
        public static double[] ItemByItemMultiply(int[] array1, double[] array2)
        {
            if (array1 == null) throw new Exception("'array1' is null");
            if (array2 == null) throw new Exception("'array2' is null");
            if (array1.Length != array1.Length) throw new Exception($"'array1' length ({array1.Length}) must be equal to 'array2' length ({array2.Length})");

            var result = new double[array1.Length];
            for (int i = 0; i < array1.Length; i++)
            {
                result[i] = array1[i] * array2[i];
            }
            return result;
        }
        public static double[] ItemByItemMultiply(double[] array1, double[] array2)
        {
            if (array1 == null) throw new Exception("'array1' is null");
            if (array2 == null) throw new Exception("'array2' is null");
            if (array1.Length != array1.Length) throw new Exception($"'array1' length ({array1.Length}) must be equal to 'array2' length ({array2.Length})");

            var result = new double[array1.Length];
            for (int i = 0; i < array1.Length; i++)
            {
                result[i] = array1[i] * array2[i];
            }
            return result;
        }

        //ITEM BY FACTOR
        public static double[] ItemByFactorMultiply(uint[] array, double factor)
        {
            if (array == null) throw new Exception("array parameter is null");

            var result = new double[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = array[i] * factor;
            }
            return result;
        }
        public static uint[] ItemByFactorMultiply(uint[] array, uint factor)
        {
            if (array == null) throw new Exception("array parameter is null");

            var result = new uint[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = array[i] * factor;
            }
            return result;
        }
        public static uint[] ItemByFactorMultiply(uint[] array, int factor)
        {
            if (array == null) throw new Exception("array parameter is null");

            var result = new uint[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = (uint)(array[i] * factor);
            }
            return result;
        }

        public static double[] ItemByFactorMultiply(int[] array, double factor)
        {
            if (array == null) throw new Exception("array parameter is null");

            var result = new double[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = array[i] * factor;
            }
            return result;
        }
        public static int[] ItemByFactorMultiply(int[] array, uint factor)
        {
            if (array == null) throw new Exception("array parameter is null");

            var result = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = (int)(array[i] * factor);
            }
            return result;
        }
        public static int[] ItemByFactorMultiply(int[] array, int factor)
        {
            if (array == null) throw new Exception("array parameter is null");

            var result = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = array[i] * factor;
            }
            return result;
        }

        public static double[] ItemByFactorMultiply(double[] array, double factor)
        {
            if (array == null) throw new Exception("array parameter is null");

            var result = new double[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = array[i] * factor;
            }
            return result;
        }
        public static double[] ItemByFactorMultiply(double[] array, int factor)
        {
            if (array == null) throw new Exception("array parameter is null");

            var result = new double[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = array[i] * factor;
            }
            return result;
        }
        public static double[] ItemByFactorMultiply(double[] array, uint factor)
        {
            if (array == null) throw new Exception("array parameter is null");

            var result = new double[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = array[i] * factor;
            }
            return result;
        }
        #endregion MULTIPLICATION

        #region ITEM INCREASE
        public static uint[] ItemsIncrease(uint[] array, uint addition)
        {
            if (array == null) throw new Exception("array parameter is null");

            var result = new uint[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = array[i] + addition;
            }
            return result;
        }
        public static int[] ItemsIncrease(uint[] array, int addition)
        {
            if (array == null) throw new Exception("array parameter is null");

            var result = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = (int)(array[i] + addition);
            }
            return result;
        }
        public static double[] ItemsIncrease(uint[] array, double addition)
        {
            if (array == null) throw new Exception("array parameter is null");

            var result = new double[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = array[i] + addition;
            }
            return result;
        }
        
        public static double[] ItemsIncrease(double[] array, uint addition)
        {
            if (array == null) throw new Exception("array parameter is null");

            var result = new double[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = array[i] + addition;
            }
            return result;
        }
        public static double[] ItemsIncrease(double[] array, int addition)
        {
            if (array == null) throw new Exception("array parameter is null");

            var result = new double[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = (array[i] + addition);
            }
            return result;
        }
        public static double[] ItemsIncrease(double[] array, double addition)
        {
            if (array == null) throw new Exception("array parameter is null");

            var result = new double[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = array[i] + addition;
            }
            return result;
        }

        public static int[] ItemsIncrease(int[] array, uint addition)
        {
            if (array == null) throw new Exception("array parameter is null");

            var result = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = (int)(array[i] + addition);
            }
            return result;
        }
        public static int[] ItemsIncrease(int[] array, int addition)
        {
            if (array == null) throw new Exception("array parameter is null");

            var result = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = array[i] + addition;
            }
            return result;
        }
        public static double[] ItemsIncrease(int[] array, double addition)
        {
            if (array == null) throw new Exception("array parameter is null");

            var result = new double[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = array[i] + addition;
            }
            return result;
        }


        public static uint[] ItemsIncrease(uint[] array, uint[] addition)
        {
            if (array == null) throw new Exception("array parameter is null");
            if (addition == null) throw new Exception("addition parameter is null");

            var result = new uint[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = array[i] + addition[i];
            }
            return result;
        }
        public static int[] ItemsIncrease(uint[] array, int[] addition)
        {
            if (array == null) throw new Exception("array parameter is null");
            if (addition == null) throw new Exception("addition parameter is null");

            var result = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = (int)(array[i] + addition[i]);
            }
            return result;
        }
        public static double[] ItemsIncrease(uint[] array, double[] addition)
        {
            if (array == null) throw new Exception("array parameter is null");
            if (addition == null) throw new Exception("addition parameter is null");

            var result = new double[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = array[i] + addition[i];
            }
            return result;
        }
        public static int[] ItemsIncrease(int[] array, int[] addition)
        {
            if (array == null) throw new Exception("array parameter is null");
            if (addition == null) throw new Exception("addition parameter is null");

            var result = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = (array[i] + addition[i]);
            }
            return result;
        }
        public static double[] ItemsIncrease(int[] array, double[] addition)
        {
            if (array == null) throw new Exception("array parameter is null");
            if (addition == null) throw new Exception("addition parameter is null");

            var result = new double[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = array[i] + addition[i];
            }
            return result;
        }
        public static double[] ItemsIncrease(double[] array, double[] addition)
        {
            if (array == null) throw new Exception("array parameter is null");
            if (addition == null) throw new Exception("addition parameter is null");

            var result = new double[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = array[i] + addition[i];
            }
            return result;
        }
        #endregion ITEM INCREASE

        // Analog to numpy.unravel_index
        public static uint[] UnravelIndex(uint index, uint[] dimensions)
        {
            double buf = index;
            var decl_dimensions = dimensions;
            Array.Reverse(decl_dimensions);
            var real_coordinates = new uint[decl_dimensions.Length];
            for (int dimensionIndex = 0; dimensionIndex < decl_dimensions.Length; dimensionIndex++)
            {
                real_coordinates[dimensionIndex] = (uint)buf % (uint)decl_dimensions[dimensionIndex];
                buf = Math.Truncate((buf / decl_dimensions[dimensionIndex]));
                if (buf == 0)
                    break;
            }
            Array.Reverse(real_coordinates);
            return real_coordinates;
        }

        // Analog to numpy.ravel_multi_index
        public static uint RavelMultiIndex(uint[] multiIndex, uint[] dimensions)
        {
            if (multiIndex == null) throw new Exception("multiIndex array parameter is null.");
            if (dimensions == null) throw new Exception("dimensions array parameter is null.");
            if (multiIndex.Length != dimensions.Length) throw new Exception($"multiIndex length ({multiIndex.Length}) must be equal to dimensions length ({dimensions.Length}).");

            for (int i = 0; i < multiIndex.Length; i++)
            {
                if (multiIndex[i] >= dimensions[i]) throw new Exception($"Index '{multiIndex[i]}' at position {i} is out of bounds. Max index at this dimension: {dimensions[i]-1}.");
            }

            uint ravelIndex = 0;
            for (int i = 0; i < dimensions.Length-1; i++)
            {
                var b = dimensions.Skip(i + 1).AsParallel().Aggregate((x, y) => x * y);
                ravelIndex += multiIndex[i] * b;
            }
            ravelIndex += multiIndex.Last();
            return ravelIndex;
        }
    }
}
