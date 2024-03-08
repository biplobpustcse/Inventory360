using Inventory360Entity;
using DAL.Interface.Select.Configuration;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Configuration
{
    public class DSelectConfigurationCode : ISelectConfigurationCode
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectConfigurationCode(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Configuration_Code> SelectCodeAll()
        {
            return _db.Configuration_Code
                .Where(x => x.CompanyId == _companyId);
        }
    }
}