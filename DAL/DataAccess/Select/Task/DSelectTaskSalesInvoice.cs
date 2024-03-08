using Inventory360Entity;
using DAL.Interface.Select.Task;
using System;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskSalesInvoice : ISelectTaskSalesInvoice
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskSalesInvoice(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_SalesInvoice> SelectSalesInvoiceAll()
        {
            return _db.Task_SalesInvoice
                .Where(x => x.CompanyId == _companyId);
        }
        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_SalesInvoiceDetail> SelectSalesInvoiceDetail()
        {
            return _db.Task_SalesInvoiceDetail.Where(x=>x.Task_SalesInvoice.CompanyId == _companyId);
        }
        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_SalesInvoiceDetailSerial> SelectSalesInvoiceDetailSerial()
        {
            return _db.Task_SalesInvoiceDetailSerial;
        }
        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool CheckSalesInvoiceIsSettledByCollection(Guid id)
        {
            return _db.Task_SalesInvoice
                .Where(x => x.CompanyId == _companyId && x.InvoiceId == id)
                .Select(s => s.InvoiceAmount - s.InvoiceDiscount - s.CollectedAmount == 0)
                .FirstOrDefault();
        }
    }
}