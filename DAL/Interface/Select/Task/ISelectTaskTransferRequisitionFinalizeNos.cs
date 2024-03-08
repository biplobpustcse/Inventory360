using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskTransferRequisitionFinalizeNos
    {
        IQueryable<Task_TransferRequisitionFinalizeNos> SelectTransferRequisitionFinalizeNosAll();
    }
}