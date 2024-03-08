using Inventory360Entity;
using DAL.Interface.Select.Task;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskPaymentMapping : ISelectTaskPaymentMapping
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskPaymentMapping(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_PaymentMapping> SelectPaymentMappingAll()
        {
            return _db.Task_PaymentMapping
                .Where(x => x.Task_Payment.CompanyId == _companyId);
        }
    }
}