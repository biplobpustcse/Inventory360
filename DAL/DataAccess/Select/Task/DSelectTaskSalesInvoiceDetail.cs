using Inventory360Entity;
using DAL.Interface.Select.Task;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskSalesInvoiceDetail : ISelectTaskSalesInvoiceDetail
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskSalesInvoiceDetail(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_SalesInvoiceDetail> SelectSalesInvoiceDetailAll()
        {
            return _db.Task_SalesInvoiceDetail
                .Where(x => x.Task_SalesInvoice.CompanyId == _companyId);
        }
    }
}