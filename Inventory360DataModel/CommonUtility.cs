using System;

namespace Inventory360DataModel
{
    public static class CommonUtility
    {
        public static long StartingIndexOfDataGrid(long pageNo, long itemQuantity)
        {
            if (pageNo == 0)
            {
                return 0;
            }
            else if (itemQuantity == 0)
            {
                return 0;
            }
            else
            {
                return ((pageNo - 1) * itemQuantity) + 1;
            }
        }

        public static long EndingIndexOfDataGrid(long startIndex, long itemQuantity, long totalRecord)
        {
            return (startIndex + (itemQuantity - 1)) < totalRecord ? (startIndex + (itemQuantity - 1)) : totalRecord;
        }

        public static long LastPageNo(long itemQuantity, long totalRecord)
        {
            return (long)Math.Ceiling(Convert.ToDouble(totalRecord) / Convert.ToDouble(itemQuantity));
        }
    }
}
