using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskRequisitionFinalize
    {
        IQueryable<Task_RequisitionFinalize> SelectRequisitionFinalizeAll();
    }
}