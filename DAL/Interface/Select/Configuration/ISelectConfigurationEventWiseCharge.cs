using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Configuration
{
    public interface ISelectConfigurationEventWiseCharge
    {
        IQueryable<Configuration_EventWiseCharge> SelectEventWiseChargeAll();
    }
}