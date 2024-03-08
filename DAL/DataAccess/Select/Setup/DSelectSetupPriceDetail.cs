using Inventory360Entity;
using DAL.Interface.Select.Setup;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Setup
{
    public class DSelectSetupPriceDetail : ISelectSetupPriceDetail
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectSetupPriceDetail(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Setup_PriceDetail> SelectProductPriceDetailAll()
        {
            return _db.Setup_PriceDetail
                .Where(x => x.Setup_Price.CompanyId == _companyId);
        }
    }
}