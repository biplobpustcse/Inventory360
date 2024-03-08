using Inventory360Entity;
using DAL.Interface.Select.Task;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskSalesInvoiceNos : ISelectTaskSalesInvoiceNos
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskSalesInvoiceNos(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_SalesInvoiceNos> SelectSalesInvoiceNosAll()
        {
            return _db.Task_SalesInvoiceNos
                .Where(x => x.CompanyId == _companyId);
        }
    }
}