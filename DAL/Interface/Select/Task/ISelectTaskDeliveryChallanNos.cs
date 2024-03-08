using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskDeliveryChallanNos
    {
        IQueryable<Task_DeliveryChallanNos> SelectDeliveryChallanNosAll();
    }
}