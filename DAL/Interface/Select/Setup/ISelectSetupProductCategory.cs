using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupProductCategory
    {
        IQueryable<Setup_ProductCategory> SelectProductCategoryAll();
        IQueryable<Setup_ProductCategory> SelectProductCategoryWithoutCheckingCompany();
    }
}
