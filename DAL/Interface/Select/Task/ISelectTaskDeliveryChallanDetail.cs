using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskDeliveryChallanDetail
    {
        IQueryable<Task_DeliveryChallanDetail> SelectDeliveryChallanDetailAll();
    }
}