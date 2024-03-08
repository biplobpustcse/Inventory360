using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupProductDimension
    {
        IQueryable<Setup_ProductDimension> SelectProductDimensionAll();
    }
}