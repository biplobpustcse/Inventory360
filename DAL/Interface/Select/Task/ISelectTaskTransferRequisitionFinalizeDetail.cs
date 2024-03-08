using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskTransferRequisitionFinalizeDetail
    {
        IQueryable<Task_TransferRequisitionFinalizeDetail> SelectRequisitionFinalizeDetailAll();
    }
}