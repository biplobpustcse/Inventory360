using Inventory360Entity;
using DAL.Interface.Select.Task;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskItemRequisitionDetail : ISelectTaskItemRequisitionDetail
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskItemRequisitionDetail(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_ItemRequisitionDetail> SelectItemRequisitionDetailAll()
        {
            return _db.Task_ItemRequisitionDetail
                .Where(x => x.Task_ItemRequisition.CompanyId == _companyId);
        }
    }
}