using Inventory360Entity;
using System;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskSalesInvoice
    {
        IQueryable<Task_SalesInvoice> SelectSalesInvoiceAll();
        IQueryable<Task_SalesInvoiceDetail> SelectSalesInvoiceDetail();
        IQueryable<Task_SalesInvoiceDetailSerial> SelectSalesInvoiceDetailSerial();
        bool CheckSalesInvoiceIsSettledByCollection(Guid id);
    }
}