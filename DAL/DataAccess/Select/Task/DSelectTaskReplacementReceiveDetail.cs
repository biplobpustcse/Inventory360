using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Select.Task;
using System;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskReplacementReceiveDetail : ISelectTaskReplacementReceiveDetail
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskReplacementReceiveDetail(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_ReplacementReceiveDetail> SelectReplacementReceiveDetailAll()
        {
            return _db.Task_ReplacementReceiveDetail.Where(x => x.Task_ReplacementReceive.CompanyId == _companyId);
        }
    }
}