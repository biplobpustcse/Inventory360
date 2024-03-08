using Inventory360Entity;
using DAL.Interface.Select.Task;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskReplacementClaimDetail_Problem : ISelectTaskReplacementClaimDetail_Problem
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskReplacementClaimDetail_Problem(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_ReplacementClaimDetail_Problem> SelectReplacementClaimDetail_ProblemAll()
        {
            return _db.Task_ReplacementClaimDetail_Problem
                .Where(x => x.Task_ReplacementClaimDetail.Task_ReplacementClaim.CompanyId == _companyId);
        }
    }
}