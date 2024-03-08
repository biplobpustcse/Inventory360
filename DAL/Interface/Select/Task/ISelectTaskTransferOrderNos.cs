using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskTransferOrderNos
    {
        IQueryable<Task_TransferOrderNos> SelectTaskTransferOrderNosAll();
    }
}