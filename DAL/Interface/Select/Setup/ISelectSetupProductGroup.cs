using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupProductGroup
    {
        IQueryable<Setup_ProductGroup> SelectProductGroupAll();
        IQueryable<Setup_ProductGroup> SelectProductGroupWithoutCheckingCompany();
    }
}
