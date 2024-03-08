using Inventory360Entity;
using DAL.Interface.Select.Setup;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Setup
{
    public class DSelectSetupEventWiseCharge : ISelectSetupEventWiseCharge
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectSetupEventWiseCharge(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Configuration_EventWiseCharge> SelectEventWiseChargeAll()
        {
            return _db.Configuration_EventWiseCharge
                .Where(x => x.CompanyId == _companyId);
        }
    }
}