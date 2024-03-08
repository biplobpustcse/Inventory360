using Inventory360Entity;
using DAL.Interface.Select.Task;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskStockAdjustmentDetailSerial : ISelectTaskStockAdjustmentDetailSerial
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskStockAdjustmentDetailSerial(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_StockAdjustmentDetailSerial> SelectStockAdjustmentDetailSerialAll()
        {
            return _db.Task_StockAdjustmentDetailSerial
                .Where(x => x.Task_StockAdjustmentDetail.Task_StockAdjustment.CompanyId == _companyId);
        }
    }
}