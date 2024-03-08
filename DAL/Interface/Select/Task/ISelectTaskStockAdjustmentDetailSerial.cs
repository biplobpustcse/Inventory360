using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskStockAdjustmentDetailSerial
    {
        IQueryable<Task_StockAdjustmentDetailSerial> SelectStockAdjustmentDetailSerialAll();
    }
}