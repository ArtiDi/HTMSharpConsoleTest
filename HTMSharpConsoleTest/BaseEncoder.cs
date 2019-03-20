using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMSharpConsoleTest
{
    class BaseEncoder
    {
        public static int[] MakeRange(int startIndex, int endIndex)
        {
            try
            {
                if (startIndex < 0) startIndex = 0;
                if (endIndex < 0) endIndex = 0;

                int[] indexRange = new int[endIndex - startIndex + 1];
                for (int value_index = startIndex, array_index = 0; value_index <= endIndex; value_index++, array_index++)
                {
                    indexRange[array_index] = value_index;
                }
                return indexRange;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
