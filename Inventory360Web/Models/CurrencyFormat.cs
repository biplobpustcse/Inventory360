using System.Collections.Generic;

namespace Inventory360Web.Models
{
    public static class CurrencyFormat
    {
        public static List<dynamic> CountryWiseCultureInfo()
        {
            List<dynamic> lists = new List<dynamic>();
            lists.Add(new { CountryName = "BANGLADESH", CurrencyCode = "BDT", CultureInfoCode = "bn-BD" });
            lists.Add(new { CountryName = "UNITED STATES OF AMERICA", CurrencyCode = "USD", CultureInfoCode = "en-US" });
            lists.Add(new { CountryName = "UNITED KINGDOM", CurrencyCode = "GBP", CultureInfoCode = "en-GB" });
            lists.Add(new { CountryName = "INDIA", CurrencyCode = "INR", CultureInfoCode = "en-IN" });
            lists.Add(new { CountryName = "CHINA", CurrencyCode = "CNY", CultureInfoCode = "zh-CN" });

            return lists;
        }
    }
}