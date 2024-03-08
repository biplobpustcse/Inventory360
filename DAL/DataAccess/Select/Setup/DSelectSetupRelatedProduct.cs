using Inventory360Entity;
using DAL.Interface.Select.Setup;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Setup
{
    public class DSelectSetupRelatedProduct : ISelectSetupRelatedProduct
    {
        private Inventory360Entities _db;
        private long _companyId;
        private long _productId;

        public DSelectSetupRelatedProduct(long productId, long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
            _productId = productId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Setup_RelatedProduct> SelectRelatedProductAll()
        {
            return _db.Setup_RelatedProduct
                .Where(x => x.Setup_Product.CompanyId == _companyId
                && x.ProductId == _productId);
        }
    }
}