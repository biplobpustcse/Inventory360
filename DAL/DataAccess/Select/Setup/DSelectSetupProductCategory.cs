using Inventory360Entity;
using DAL.Interface.Select.Setup;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Setup
{
    public class DSelectSetupProductCategory : ISelectSetupProductCategory
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectSetupProductCategory(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Setup_ProductCategory> SelectProductCategoryAll()
        {
            return _db.Setup_ProductCategory
                .Where(x => x.CompanyId == _companyId);
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Setup_ProductCategory> SelectProductCategoryWithoutCheckingCompany()
        {
            return _db.Setup_ProductCategory;
        }
    }
}