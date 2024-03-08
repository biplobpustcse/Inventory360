using System;

namespace Inventory360DataModel
{
    public static class MyConversion
    {
        public static DateTime? ConvertDateStringToDate(string date)
        {
            try
            {
                if (string.IsNullOrEmpty(date))
                    return null;

                string[] splittedDate = date.Split('/');
                return new DateTime(Convert.ToInt32(splittedDate[2]), Convert.ToInt32(splittedDate[0]), Convert.ToInt32(splittedDate[1]));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
