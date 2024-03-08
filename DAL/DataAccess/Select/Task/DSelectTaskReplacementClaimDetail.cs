using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Select.Task;
using System;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskReplacementClaimDetail : ISelectTaskReplacementClaimDetail
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskReplacementClaimDetail(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_ReplacementClaimDetail> SelectReplacementClaimDetailAll()
        {
            return _db.Task_ReplacementClaimDetail.Where(x => x.Task_ReplacementClaim.CompanyId == _companyId);
        }
    }
}