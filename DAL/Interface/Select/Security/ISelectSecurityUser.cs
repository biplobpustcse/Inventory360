using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Security
{
    public interface ISelectSecurityUser
    {
        IQueryable<Security_User> SelectSecurityUser();
        IQueryable<Security_UserLocation> SelectSecurityUserLocation();
    }
}
