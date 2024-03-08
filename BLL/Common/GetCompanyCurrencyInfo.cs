using Inventory360DataModel;
using DAL.DataAccess.Select.Setup;
using DAL.Interface.Select.Setup;
using System;
using System.Linq;

namespace BLL.Common
{
    public static class GetCompanyCurrencyInfo
    {
        public static CommonCompanyCurrency CompanyCurrencyInfo(long companyId)
        {
            try
            {
                ISelectSetupCompany iSelectSetupCompany = new DSelectSetupCompany();

                return iSelectSetupCompany.SelectCompanyAll()
                    .Where(x => x.CompanyId == companyId)
                    .Select(s => new CommonCompanyCurrency
                    {
                        BaseCurrency = s.BaseCurrency,
                        Currency1 = s.Currency1,
                        Currency2 = s.Currency2
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