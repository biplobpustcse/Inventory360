using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Select.Task;
using System;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskReplacementReceive : ISelectTaskReplacementReceive
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskReplacementReceive(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_ReplacementReceive> SelectTaskReplacementReceiveAll()
        {
            return _db.Task_ReplacementReceive.Where(x => x.CompanyId == _companyId);
        }
    }
}