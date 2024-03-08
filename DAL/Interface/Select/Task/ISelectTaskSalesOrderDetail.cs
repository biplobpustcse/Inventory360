using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskSalesOrderDetail
    {
        IQueryable<Task_SalesOrderDetail> SelectSalesOrderDetailAll();
    }
}