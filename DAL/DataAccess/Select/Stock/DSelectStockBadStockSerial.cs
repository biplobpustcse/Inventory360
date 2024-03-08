using Inventory360Entity;
using DAL.Interface.Select.Stock;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Stock
{
    public class DSelectStockBadStockSerial : ISelectStockBadStockSerial
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectStockBadStockSerial(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Stock_BadStockSerial> SelectBadStockSerialAll()
        {
            return _db.Stock_BadStockSerial
                .Where(x => x.Stock_BadStock.CompanyId == _companyId);
        }
    }
}