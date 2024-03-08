using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Configuration
{
    public interface ISelectConfigurationOperationalEvent
    {
        IQueryable<Configuration_OperationalEvent> SelectOperationalEventAll();
    }
}