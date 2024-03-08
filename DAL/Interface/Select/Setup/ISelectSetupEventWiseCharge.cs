using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupEventWiseCharge
    {
        IQueryable<Configuration_EventWiseCharge> SelectEventWiseChargeAll();
    }
}