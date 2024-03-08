using Inventory360Entity;
using DAL.Interface.Select.Task;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskRequisitionFinalizeDetail : ISelectTaskRequisitionFinalizeDetail
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskRequisitionFinalizeDetail(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_RequisitionFinalizeDetail> SelectRequisitionFinalizeDetailAll()
        {
            return _db.Task_RequisitionFinalizeDetail
                .Where(x => x.Task_RequisitionFinalize.CompanyId == _companyId);
        }
    }
}