using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskPurchaseOrder
    {
        IQueryable<Task_PurchaseOrder> SelectPurchaseOrderAll();
    }
}