using Inventory360Entity;
using DAL.Interface.Select.Setup;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Setup
{
    public class DSelectSetupProductDimension : ISelectSetupProductDimension
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectSetupProductDimension(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Setup_ProductDimension> SelectProductDimensionAll()
        {
            return _db.Setup_ProductDimension
                .Where(x => x.Setup_Product.CompanyId == _companyId);
        }
    }
}