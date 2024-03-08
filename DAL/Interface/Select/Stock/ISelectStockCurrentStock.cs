using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Stock
{
    public interface ISelectStockCurrentStock
    {
        IQueryable<Stock_CurrentStock> SelectCurrentStockAll();
    }
}