using Inventory360Entity;
using DAL.Interface.Select.Configuration;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Configuration
{
    public class DSelectConfigurationCurrencyRate : ISelectConfigurationCurrencyRate
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectConfigurationCurrencyRate(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Configuration_CurrencyRate> SelectCurrencyRateAll()
        {
            return _db.Configuration_CurrencyRate
                .Where(x => x.CompanyId == _companyId);
        }
    }
}