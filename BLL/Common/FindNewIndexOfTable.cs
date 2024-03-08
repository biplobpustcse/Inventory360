using System.Collections.Generic;

namespace BLL.Common
{
    public static class FindNewIndexOfTable
    {
        public static long FindNewIndexForTable(List<long> existingIndexs)
        {
            long newIndex = 1;
            foreach (long index in existingIndexs)
            {
                if (!existingIndexs.Contains(newIndex))
                {
                    break;
                }

                ++newIndex;
            }

            return newIndex;
        }
    }
}