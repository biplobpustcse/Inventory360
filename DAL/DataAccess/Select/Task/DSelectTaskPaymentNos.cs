using Inventory360Entity;
using DAL.Interface.Select.Task;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskPaymentNos : ISelectTaskPaymentNos
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskPaymentNos(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_PaymentNos> SelectPaymentNosAll()
        {
            return _db.Task_PaymentNos
                .Where(x => x.CompanyId == _companyId);
        }
    }
}