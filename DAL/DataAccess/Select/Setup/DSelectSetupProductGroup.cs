using Inventory360Entity;
using DAL.Interface.Select.Setup;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Setup
{
    public class DSelectSetupProductGroup : ISelectSetupProductGroup
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectSetupProductGroup(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Setup_ProductGroup> SelectProductGroupAll()
        {
            return _db.Setup_ProductGroup
                .Where(x => x.CompanyId == _companyId);
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Setup_ProductGroup> SelectProductGroupWithoutCheckingCompany()
        {
            return _db.Setup_ProductGroup;
        }
    }
}