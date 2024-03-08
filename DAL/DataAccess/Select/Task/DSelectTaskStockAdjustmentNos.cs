using Inventory360Entity;
using DAL.Interface.Select.Task;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskStockAdjustmentNos : ISelectTaskStockAdjustmentNos
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskStockAdjustmentNos(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_StockAdjustmentNos> SelectStockAdjustmentNosAll()
        {
            return _db.Task_StockAdjustmentNos
                .Where(x => x.CompanyId == _companyId);
        }
    }
}