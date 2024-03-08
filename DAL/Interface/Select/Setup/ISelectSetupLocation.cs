using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupLocation
    {
        IQueryable<Setup_Location> SelectLocationAll();
    }
}
