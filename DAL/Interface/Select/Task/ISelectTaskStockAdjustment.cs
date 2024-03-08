using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskStockAdjustment
    {
        IQueryable<Task_StockAdjustment> SelectStockAdjustmentAll();
    }
}