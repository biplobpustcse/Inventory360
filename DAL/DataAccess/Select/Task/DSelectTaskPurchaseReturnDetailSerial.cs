using Inventory360Entity;
using DAL.Interface.Select.Task;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskPurchaseReturnDetailSerial : ISelectTaskPurchaseReturnDetailSerial
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskPurchaseReturnDetailSerial(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_PurchaseReturnDetailSerial> SelectPurchaseReturnDetailSerialAll()
        {
            return _db.Task_PurchaseReturnDetailSerial
                .Where(x => x.Task_PurchaseReturnDetail.Task_PurchaseReturn.CompanyId == _companyId);
        }
    }
}