using Inventory360Entity;
using DAL.Interface.Select.Setup;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Setup
{
    public class DSelectSetupProductSubGroup : ISelectSetupProductSubGroup
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectSetupProductSubGroup(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Setup_ProductSubGroup> SelectProductSubGroupAll()
        {
            return _db.Setup_ProductSubGroup
                .Where(x => x.CompanyId == _companyId);
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Setup_ProductSubGroup> SelectProductSubGroupWithoutCheckingCompany()
        {
            return _db.Setup_ProductSubGroup;
        }
    }
}