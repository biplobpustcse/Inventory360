using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Select.Task;
using System;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskReplacementClaim : ISelectTaskReplacementClaim
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskReplacementClaim(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_ReplacementClaim> SelectTaskReplacementClaimAll()
        {
            return _db.Task_ReplacementClaim.Where(x => x.CompanyId == _companyId);
        }              
    }
}