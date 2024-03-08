using Inventory360Entity;
using System;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskTransferRequisitionFinalize
    {
        IQueryable<Task_TransferRequisitionFinalize> SelectStockTransferRequisitionFinalizeAll();
        IQueryable<Task_TransferRequisitionFinalize> SelectStockTransferRequisitionFinalize(Guid Id);
        
    }
}