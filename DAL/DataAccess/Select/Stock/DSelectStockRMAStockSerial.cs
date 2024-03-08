using Inventory360Entity;
using DAL.Interface.Select.Stock;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Stock
{
    public class DSelectStockRMAStockSerial : ISelectStockRMAStockSerial
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectStockRMAStockSerial(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Stock_RMAStockSerial> SelectRMAStockSerialAll()
        {
            return _db.Stock_RMAStockSerial
                .Where(x => x.Stock_RMAStock.CompanyId == _companyId);
        }
    }
}