using Inventory360Entity;
using DAL.Interface.Delete.Setup;
using System;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Delete.Setup
{
    public class DDeleteSetupPriceDetail : IDeleteSetupPriceDetail
    {
        private Inventory360Entities _db;

        public DDeleteSetupPriceDetail()
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool DeletePriceDetail(long priceId, long companyId)
        {
            try
            {
                _db.Setup_PriceDetail
                    .RemoveRange(
                        _db.Setup_PriceDetail
                        .Where(x => x.PriceId == priceId
                        && x.Setup_Price.CompanyId == companyId)
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