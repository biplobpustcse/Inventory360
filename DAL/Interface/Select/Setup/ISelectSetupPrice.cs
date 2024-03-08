using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupPrice
    {
        IQueryable<Setup_Price> SelectProductPriceAll();
    }
}