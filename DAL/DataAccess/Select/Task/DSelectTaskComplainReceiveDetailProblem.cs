using Inventory360Entity;
using DAL.Interface.Select.Task;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskComplainReceiveDetailProblem : ISelectTaskComplainReceiveDetailProblem
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskComplainReceiveDetailProblem(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_ComplainReceiveDetail_Problem> SelectComplainReceiveDetailProblemAll()
        {
            return _db.Task_ComplainReceiveDetail_Problem
                .Where(x => x.Task_ComplainReceiveDetail.Task_ComplainReceive.CompanyId == _companyId);
        }
    }
}