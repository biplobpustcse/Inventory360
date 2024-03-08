using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskImportedStockInDetailSerial
    {
        IQueryable<Task_ImportedStockInDetailSerial> SelectImportedStockInDetailSerialAll();
    }
}