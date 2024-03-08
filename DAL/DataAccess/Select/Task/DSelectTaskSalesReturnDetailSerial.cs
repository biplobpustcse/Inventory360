using Inventory360Entity;
using DAL.Interface.Select.Task;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskSalesReturnDetailSerial : ISelectTaskSalesReturnDetailSerial
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskSalesReturnDetailSerial(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_SalesReturnDetailSerial> SelectSalesReturnDetailSerialAll()
        {
            return _db.Task_SalesReturnDetailSerial
                .Where(x => x.Task_SalesReturnDetail.Task_SalesReturn.CompanyId == _companyId);
        }
    }
}