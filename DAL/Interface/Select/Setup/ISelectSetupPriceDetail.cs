using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupPriceDetail
    {
        IQueryable<Setup_PriceDetail> SelectProductPriceDetailAll();
    }
}