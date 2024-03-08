using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupCustomerGroup
    {
        IQueryable<Setup_CustomerGroup> SelectCustomerGroupAll();
    }
}
