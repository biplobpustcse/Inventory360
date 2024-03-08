using Inventory360DataModel;
using DAL.DataAccess.Select.Configuration;
using DAL.Interface.Select.Configuration;
using System;
using System.Linq;

namespace BLL.Common
{
    public static class GetCurrencyRateInfo
    {
        public static CommonCurrencyRate GetCurrencyRate(long companyId)
        {
            try
            {
                ISelectConfigurationCurrencyRate iSelectConfigurationCurrencyRate = new DSelectConfigurationCurrencyRate(companyId);

                return iSelectConfigurationCurrencyRate.SelectCurrencyRateAll()
                .Select(s => new CommonCurrencyRate
                {
                    BaseCurrencyRate = s.BaseCurrencyRate,
                    Currency1Rate = s.Currency1Rate,
                    Currency2Rate = s.Currency2Rate
                })
                .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}