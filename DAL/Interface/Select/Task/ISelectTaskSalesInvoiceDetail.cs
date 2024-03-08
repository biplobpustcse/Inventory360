using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskSalesInvoiceDetail
    {
        IQueryable<Task_SalesInvoiceDetail> SelectSalesInvoiceDetailAll();
    }
}