using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectStockAdjustmentDetailSerial
    {
        IQueryable<Task_StockAdjustmentDetailSerial> SelectStockAdjustmentDetailSerialAll();
    }
}