using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupBrand
    {
        IQueryable<Setup_Brand> SelectBrandAll();
        IQueryable<Setup_Brand> SelectBrandWithoutCheckingCompany();
    }
}
