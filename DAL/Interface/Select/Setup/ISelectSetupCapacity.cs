using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupCapacity
    {
        IQueryable<Setup_Capacity> SelectCapacityAll();
    }
}
