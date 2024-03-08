using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskItemRequisitionNos
    {
        IQueryable<Task_ItemRequisitionNos> SelectItemRequisitionNosAll();
    }
}