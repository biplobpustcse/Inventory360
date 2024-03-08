using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupProductSubGroup
    {
        IQueryable<Setup_ProductSubGroup> SelectProductSubGroupAll();
        IQueryable<Setup_ProductSubGroup> SelectProductSubGroupWithoutCheckingCompany();
    }
}