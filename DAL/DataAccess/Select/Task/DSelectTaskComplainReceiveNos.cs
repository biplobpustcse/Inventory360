using Inventory360Entity;
using DAL.Interface.Select.Task;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskComplainReceiveNos : ISelectTaskComplainReceiveNos
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskComplainReceiveNos(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_ComplainReceiveNos> SelectComplainReceiveNosAll()
        {
            return _db.Task_ComplainReceiveNos
                .Where(x => x.CompanyId == _companyId);
        }
    }
}