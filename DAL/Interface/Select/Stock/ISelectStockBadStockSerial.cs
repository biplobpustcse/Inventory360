using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Stock
{
    public interface ISelectStockBadStockSerial
    {
        IQueryable<Stock_BadStockSerial> SelectBadStockSerialAll();
    }
}