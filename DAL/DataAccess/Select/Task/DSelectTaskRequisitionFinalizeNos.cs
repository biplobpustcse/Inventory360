using Inventory360Entity;
using DAL.Interface.Select.Task;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskRequisitionFinalizeNos : ISelectTaskRequisitionFinalizeNos
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskRequisitionFinalizeNos(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_RequisitionFinalizeNos> SelectRequisitionFinalizeNosAll()
        {
            return _db.Task_RequisitionFinalizeNos
                .Where(x => x.CompanyId == _companyId);
        }
    }
}