using Inventory360Entity;
using DAL.Interface.Select.Stock;
using DAL.Interface.Select.Task;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Stock
{
    public class DSelectStockItemTransferDetail : ISelectStockItemTransferDetail
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectStockItemTransferDetail(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_ItemRequisitionDetail> SelectItemTransferDetailAll()
        {
            return _db.Task_ItemRequisitionDetail
                .Where(x => x.Task_ItemRequisition.CompanyId == _companyId);
        }
    }
}