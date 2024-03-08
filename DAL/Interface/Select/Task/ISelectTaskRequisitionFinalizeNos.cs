using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskRequisitionFinalizeNos
    {
        IQueryable<Task_RequisitionFinalizeNos> SelectRequisitionFinalizeNosAll();
    }
}