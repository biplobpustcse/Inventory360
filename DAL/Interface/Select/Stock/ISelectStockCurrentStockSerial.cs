using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Stock
{
    public interface ISelectStockCurrentStockSerial
    {
        IQueryable<Stock_CurrentStockSerial> SelectCurrentStockSerialAll();
    }
}