using Inventory360Entity;
using DAL.Interface.Select.Stock;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Stock
{
    public class DSelectStockCurrentStock : ISelectStockCurrentStock
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectStockCurrentStock(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Stock_CurrentStock> SelectCurrentStockAll()
        {
            return _db.Stock_CurrentStock
                .Where(x => x.CompanyId == _companyId);
        }
    }
}