using System;
using System.Configuration;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace Inventory360Web.Controllers
{
    public class BaseController : Controller
    {
        protected string _footerCompanyName = string.Empty;
        protected string _footerProductName = string.Empty;

        public BaseController()
        {
            _footerCompanyName = "Inventory360 Solution";
            _footerProductName = "Inventory 360" + string.Format("\u00B0");
        }

        protected string PageHeader(string printedBy)
        {
            return "--header-left \"Print Date : \"" + DateTime.Now.Date.ToString("dd-MMM-yyyy") + "\" [time], By : " + printedBy + "\" " + "--header-right \"" + _footerProductName + " | Page : [page] of [toPage]\" --header-center \"Developed By : " + _footerCompanyName + "\" --header-line --header-font-size \"9\" --header-font-name \"calibri light\" ";
        }

        protected string PageHeaderFooter(string printedBy, string pageTag)
        {
            return "--footer-left \"Print Date : \"" + DateTime.Now.Date.ToString("dd-MMM-yyyy") + "\" [time], By : " + printedBy + "\" " + "--footer-right \"" + _footerProductName + " | Page : [page] of [toPage]\" --footer-center \"Developed By : " + _footerCompanyName + "\" --footer-line --footer-font-size \"9\" --footer-font-name \"calibri light\" --header-right \"" + pageTag + "\" --header-font-size \"9\" --header-font-name \"calibri light\" ";
        }

        protected string AmountInWord(string currencyType, decimal amount)
        {
            decimal decimalValue = amount - Math.Floor(amount);

            return AmountInWordForIntValue(currencyType, Math.Floor(amount)) + (currencyType == "BDT" ? " Taka " : (currencyType == "USD" ? " Dollar " : string.Empty))
                + (decimalValue > 0 ? (AmountInWordForIntValue(currencyType, (decimalValue) * 100) + (currencyType == "BDT" ? " Paisa" : (currencyType == "USD" ? " Cent" : string.Empty))) : string.Empty)
                + " Only";
        }

        private string AmountInWordForIntValue(string currencyType, decimal amount)
        {
            if (currencyType == "BDT")
            {
                var koti = Math.Floor(amount / 10000000); /* Koti */
                amount -= koti * 10000000;
                var lakh = Math.Floor(amount / 100000);  /* lakh  */
                amount -= lakh * 100000;
                var thousand = Math.Floor(amount / 1000);     /* Thousands (kilo) */
                amount -= thousand * 1000;
                var hundred = Math.Floor(amount / 100);      /* Hundreds (hecto) */
                amount -= hundred * 100;
                var Dn = Math.Floor(amount / 10);       /* Tens (deca) */
                var n = amount % 10;               /* Ones */

                var res = "";
                if (koti > 0)
                {
                    res += AmountInWordForIntValue(currencyType, koti) + " Crore ";
                }
                if (lakh > 0)
                {
                    res += AmountInWordForIntValue(currencyType, lakh) + " Lakh";
                }
                if (thousand > 0)
                {
                    res += (string.IsNullOrEmpty(res) ? "" : " ") + AmountInWordForIntValue(currencyType, thousand) + " Thousand";
                }
                if (hundred > 0)
                {
                    res += (string.IsNullOrEmpty(res) ? "" : " ") + AmountInWordForIntValue(currencyType, hundred) + " Hundred";
                }

                string[] ones = { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eightteen", "Nineteen" };
                string[] tens = { "", "", "Twenty", "Thirty", "Fourty", "Fifty", "Sixty", "Seventy", "Eigthy", "Ninety" };

                if (Dn > 0 || n > 0)
                {
                    if (!string.IsNullOrEmpty(res))
                    {
                        res += " and ";
                    }

                    if (Dn < 2)
                    {
                        res += ones[(int)(Dn * 10 + n)];
                    }
                    else
                    {
                        res += tens[(int)Dn];
                        if (n > 0)
                        {
                            res += "-" + ones[(int)n];
                        }
                    }
                }
                if (string.IsNullOrEmpty(res))
                {
                    res = "zero";
                }

                return res;
            }
            else if (currencyType == "USD")
            {
                var billion = Math.Floor(amount / 1000000000); /* billion */
                amount -= billion * 1000000000;
                var million = Math.Floor(amount / 1000000);  /* million  */
                amount -= million * 1000000;
                var thousand = Math.Floor(amount / 1000);     /* Thousands (kilo) */
                amount -= thousand * 1000;
                var hundred = Math.Floor(amount / 100);      /* Hundreds (hecto) */
                amount -= hundred * 100;
                var Dn = Math.Floor(amount / 10);       /* Tens (deca) */
                var n = amount % 10;               /* Ones */

                var res = "";
                if (billion > 0)
                {
                    res += AmountInWordForIntValue(currencyType, billion) + " Billion ";
                }
                if (million > 0)
                {
                    res += AmountInWordForIntValue(currencyType, million) + " Million";
                }
                if (thousand > 0)
                {
                    res += (string.IsNullOrEmpty(res) ? "" : " ") + AmountInWordForIntValue(currencyType, thousand) + " Thousand";
                }
                if (hundred > 0)
                {
                    res += (string.IsNullOrEmpty(res) ? "" : " ") + AmountInWordForIntValue(currencyType, hundred) + " Hundred";
                }

                string[] ones = { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eightteen", "Nineteen" };
                string[] tens = { "", "", "Twenty", "Thirty", "Fourty", "Fifty", "Sixty", "Seventy", "Eigthy", "Ninety" };

                if (Dn > 0 || n > 0)
                {
                    if (!string.IsNullOrEmpty(res))
                    {
                        res += " and ";
                    }

                    if (Dn < 2)
                    {
                        res += ones[(int)(Dn * 10 + n)];
                    }
                    else
                    {
                        res += tens[(int)Dn];
                        if (n > 0)
                        {
                            res += "-" + ones[(int)n];
                        }
                    }
                }
                if (string.IsNullOrEmpty(res))
                {
                    res = "zero";
                }

                return res;
            }
            else
            {
                return "";
            }
        }
    }
}