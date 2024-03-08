using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Stock
{
    public interface ISelectStockItemTransferDetail
    {
        IQueryable<Task_ItemRequisitionDetail> SelectItemTransferDetailAll();
    }
}