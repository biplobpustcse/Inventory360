using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskStockAdjustmentNos
    {
        IQueryable<Task_StockAdjustmentNos> SelectStockAdjustmentNosAll();
    }
}