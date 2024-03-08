using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskStockAdjustmentDetail
    {
        IQueryable<Task_StockAdjustmentDetail> SelectStockAdjustmentDetailAll();
    }
}