using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskSalesInvoiceDetailSerial
    {
        IQueryable<Task_SalesInvoiceDetailSerial> SelectSalesInvoiceDetailSerialAll();
    }
}