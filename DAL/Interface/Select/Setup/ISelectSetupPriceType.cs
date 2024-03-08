using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupPriceType
    {
        IQueryable<Setup_PriceType> SelectPriceTypeAll();
    }
}