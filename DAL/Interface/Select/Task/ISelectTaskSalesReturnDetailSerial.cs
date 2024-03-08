using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskSalesReturnDetailSerial
    {
        IQueryable<Task_SalesReturnDetailSerial> SelectSalesReturnDetailSerialAll();
    }
}