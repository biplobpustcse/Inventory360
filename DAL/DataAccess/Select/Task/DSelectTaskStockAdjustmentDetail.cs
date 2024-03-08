using Inventory360Entity;
using DAL.Interface.Select.Task;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskStockAdjustmentDetail : ISelectTaskStockAdjustmentDetail
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskStockAdjustmentDetail(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_StockAdjustmentDetail> SelectStockAdjustmentDetailAll()
        {
            return _db.Task_StockAdjustmentDetail
                .Where(x => x.Task_StockAdjustment.CompanyId == _companyId);
        }
    }
}