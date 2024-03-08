using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Configuration
{
    public interface ISelectConfigurationOperationalEventDetail
    {
        IQueryable<Configuration_OperationalEventDetail> SelectOperationalEventDetailAll();
    }
}