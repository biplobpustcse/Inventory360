using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskPurchaseReturnDetailSerial
    {
        IQueryable<Task_PurchaseReturnDetailSerial> SelectPurchaseReturnDetailSerialAll();
    }
}