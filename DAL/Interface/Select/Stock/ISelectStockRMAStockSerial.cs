using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Stock
{
    public interface ISelectStockRMAStockSerial
    {
        IQueryable<Stock_RMAStockSerial> SelectRMAStockSerialAll();
    }
}