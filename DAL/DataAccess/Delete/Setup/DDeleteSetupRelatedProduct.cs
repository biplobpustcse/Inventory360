using Inventory360Entity;
using DAL.Interface.Delete.Setup;
using System;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Delete.Setup
{
    public class DDeleteSetupRelatedProduct : IDeleteSetupRelatedProduct
    {
        private Inventory360Entities _db;

        public DDeleteSetupRelatedProduct()
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool DeleteRelatedProduct(long productId, long relatedProductId, long companyId)
        {
            try
            {
                _db.Setup_RelatedProduct
                    .RemoveRange(
                        _db.Setup_RelatedProduct
                        .Where(x => x.ProductId == productId
                        && x.RelatedProductId == relatedProductId
                        && x.Setup_Product1.CompanyId == companyId)
                    );
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}