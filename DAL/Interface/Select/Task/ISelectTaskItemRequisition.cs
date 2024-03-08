using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskItemRequisition
    {
        IQueryable<Task_ItemRequisition> SelectItemRequisitionAll();
    }
}