using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskRequisitionFinalizeDetail
    {
        IQueryable<Task_RequisitionFinalizeDetail> SelectRequisitionFinalizeDetailAll();
    }
}