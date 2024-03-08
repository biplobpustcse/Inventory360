using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskSalesOrderNos
    {
        IQueryable<Task_SalesOrderNos> SelectSalesOrderNosAll();
    }
}