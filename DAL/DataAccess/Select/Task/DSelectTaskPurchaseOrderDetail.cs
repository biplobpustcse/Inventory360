using Inventory360Entity;
using DAL.Interface.Select.Task;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskPurchaseOrderDetail : ISelectTaskPurchaseOrderDetail
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskPurchaseOrderDetail(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_PurchaseOrderDetail> SelectPurchaseOrderDetailAll()
        {
            return _db.Task_PurchaseOrderDetail
                .Where(x => x.Task_PurchaseOrder.CompanyId == _companyId);
        }
    }
}