using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskPurchaseOrderNos
    {
        IQueryable<Task_PurchaseOrderNos> SelectPurchaseOrderNosAll();
    }
}