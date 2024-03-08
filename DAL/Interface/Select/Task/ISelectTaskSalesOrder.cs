using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskSalesOrder
    {
        IQueryable<Task_SalesOrder> SelectSalesOrderAll();
    }
}