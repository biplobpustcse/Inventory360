using Inventory360DataModel;
using BLL.Common;
using DAL.DataAccess.Select.Setup;
using DAL.Interface.Select.Setup;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.DropDown
{
    public class DropDownCurrency
    {
        public List<CommonResultList> SelectCompanyCurrencyByCompanyIdForDropdown(long companyId)
        {
            try
            {
                List<CommonResultList> items = new List<CommonResultList>();
                ISelectSetupCompany iSelectSetupCompany = new DSelectSetupCompany();

                var lists = iSelectSetupCompany.SelectCompanyAll()
                    .Where(x => x.CompanyId == companyId)
                    .FirstOrDefault();

                string currSymbol;
                if (!string.IsNullOrEmpty(lists.BaseCurrency))
                {
                    CurrencyTools.TryGetCurrencySymbol(lists.BaseCurrency, out currSymbol);
                    items.Add(new CommonResultList { Item = currSymbol + " - " + lists.BaseCurrency, Value = lists.BaseCurrency });
                }

                if (!string.IsNullOrEmpty(lists.Currency1))
                {
                    CurrencyTools.TryGetCurrencySymbol(lists.Currency1, out currSymbol);
                    items.Add(new CommonResultList { Item = currSymbol + " - " + lists.Currency1, Value = lists.Currency1 });
                }

                if (!string.IsNullOrEmpty(lists.Currency2))
                {
                    CurrencyTools.TryGetCurrencySymbol(lists.Currency2, out currSymbol);
                    items.Add(new CommonResultList { Item = currSymbol + " - " + lists.Currency2, Value = lists.Currency2 });
                }

                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public dynamic SelectCurrencyExchangeRateByCompanyIdAndCurrencyForDropdown(string currency, long companyId)
        {
            try
            {
                decimal exchangeRate = 0;                

                var currencyInfo = GetCompanyCurrencyInfo.CompanyCurrencyInfo(companyId);
                var currencyRate = GetCurrencyRateInfo.GetCurrencyRate(companyId);

                if (currency.Equals(currencyInfo.BaseCurrency))
                {
                    exchangeRate = currencyRate.BaseCurrencyRate;
                }
                else if (currency.Equals(currencyInfo.Currency1))
                {
                    exchangeRate = currencyRate.Currency1Rate;
                }
                else if (currency.Equals(currencyInfo.Currency2))
                {
                    exchangeRate = currencyRate.Currency2Rate;
                }

                return new
                {
                    ExchangeRate = exchangeRate,
                    BaseCurrency = currencyInfo.BaseCurrency
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}