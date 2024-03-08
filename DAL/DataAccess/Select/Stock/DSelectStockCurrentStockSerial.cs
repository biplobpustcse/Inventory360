using Inventory360Entity;
using DAL.Interface.Select.Stock;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Stock
{
    public class DSelectStockCurrentStockSerial : ISelectStockCurrentStockSerial
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectStockCurrentStockSerial(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Stock_CurrentStockSerial> SelectCurrentStockSerialAll()
        {
            return _db.Stock_CurrentStockSerial
                .Where(x => x.Stock_CurrentStock.CompanyId == _companyId);
        }
    }
}