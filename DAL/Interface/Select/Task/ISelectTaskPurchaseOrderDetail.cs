using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskPurchaseOrderDetail
    {
        IQueryable<Task_PurchaseOrderDetail> SelectPurchaseOrderDetailAll();
    }
}