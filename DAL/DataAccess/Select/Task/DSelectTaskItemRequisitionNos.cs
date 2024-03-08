using Inventory360Entity;
using DAL.Interface.Select.Task;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskItemRequisitionNos : ISelectTaskItemRequisitionNos
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskItemRequisitionNos(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_ItemRequisitionNos> SelectItemRequisitionNosAll()
        {
            return _db.Task_ItemRequisitionNos
                .Where(x => x.CompanyId == _companyId);
        }
    }
}