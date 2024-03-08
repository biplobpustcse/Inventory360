using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskSalesInvoiceNos
    {
        IQueryable<Task_SalesInvoiceNos> SelectSalesInvoiceNosAll();
    }
}