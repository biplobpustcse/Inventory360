using Inventory360Entity;
using DAL.Interface.Select.Task;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskPurchaseOrderNos : ISelectTaskPurchaseOrderNos
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskPurchaseOrderNos(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_PurchaseOrderNos> SelectPurchaseOrderNosAll()
        {
            return _db.Task_PurchaseOrderNos
                .Where(x => x.CompanyId == _companyId);
        }
    }
}