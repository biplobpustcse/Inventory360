using Inventory360Entity;
using DAL.Interface.Select.Setup;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Setup
{
    public class DSelectSetupCustomerGroup : ISelectSetupCustomerGroup
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectSetupCustomerGroup(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Setup_CustomerGroup> SelectCustomerGroupAll()
        {
            return _db.Setup_CustomerGroup
                .Where(x => x.CompanyId == _companyId);
        }
    }
}
