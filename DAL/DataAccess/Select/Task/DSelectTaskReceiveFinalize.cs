using Inventory360Entity;
using DAL.Interface.Select.Task;
using System;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskReceiveFinalize : ISelectTaskReceiveFinalize
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskReceiveFinalize(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_ReceiveFinalize> SelectReceiveFinalizeAll()
        {
            return _db.Task_ReceiveFinalize
                .Where(x => x.CompanyId == _companyId);
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool CheckReceiveFinalizeIsSettledByPayment(Guid id)
        {
            return _db.Task_ReceiveFinalize
                .Where(x => x.CompanyId == _companyId && x.FinalizeId == id)
                .Select(s => s.FinalizeAmount - s.PaidAmount == 0)
                .FirstOrDefault();
        }
    }
}