using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupEmployee
    {
        IQueryable<Setup_Employee> SelectEmployeeAll();
    }
}
