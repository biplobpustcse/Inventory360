using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskItemRequisitionDetail
    {
        IQueryable<Task_ItemRequisitionDetail> SelectItemRequisitionDetailAll();
    }
}