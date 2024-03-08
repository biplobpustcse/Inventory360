using Inventory360Entity;
using DAL.Interface.Select.Task;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskSalesInvoiceDetailSerial : ISelectTaskSalesInvoiceDetailSerial
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskSalesInvoiceDetailSerial(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_SalesInvoiceDetailSerial> SelectSalesInvoiceDetailSerialAll()
        {
            return _db.Task_SalesInvoiceDetailSerial
                .Where(x => x.Task_SalesInvoiceDetail.Task_SalesInvoice.CompanyId == _companyId);
        }
    }
}