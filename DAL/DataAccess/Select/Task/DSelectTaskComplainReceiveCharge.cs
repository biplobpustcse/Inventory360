using Inventory360Entity;
using DAL.Interface.Select.Task;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskComplainReceiveCharge : ISelectTaskComplainReceiveCharge
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskComplainReceiveCharge(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_ComplainReceive_Charge> SelectTaskComplainReceiveChargeAll()
        {
            return _db.Task_ComplainReceive_Charge
                .Where(x => x.Task_ComplainReceive.CompanyId == _companyId);
        }
    }
}