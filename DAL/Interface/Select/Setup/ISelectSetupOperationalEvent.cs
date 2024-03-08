using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupOperationalEvent
    {
        IQueryable<Configuration_OperationalEvent> SelectOperationalEventAll();
    }
}