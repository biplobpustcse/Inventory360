using Inventory360DataModel;
using System;
using System.Linq;
using System.Security.Claims;

namespace Inventory360API_V2
{
    public class GetUserIdentityInfo
    {
        public static UserIdentityInfo GetUserInfo(ClaimsIdentity identity)
        {
            UserIdentityInfo info = new UserIdentityInfo();

            info.CompanyId = Convert.ToInt64(identity.Claims
                .Where(x => x.Type == "companyId")
                .Select(s => s.Value)
                .FirstOrDefault());

            info.CompanyCode = identity.Claims
                .Where(x => x.Type == "companyCode")
                .Select(s => s.Value)
                .FirstOrDefault();

            info.CompanyName = identity.Claims
                .Where(x => x.Type == "companyName")
                .Select(s => s.Value)
                .FirstOrDefault();

            info.LocationId = Convert.ToInt64(identity.Claims
                .Where(x => x.Type == "locationId")
                .Select(s => s.Value)
                .FirstOrDefault());

            info.LocationCode = identity.Claims
                .Where(x => x.Type == "locationCode")
                .Select(s => s.Value)
                .FirstOrDefault();

            info.LocationName = identity.Claims
                .Where(x => x.Type == "locationName")
                .Select(s => s.Value)
                .FirstOrDefault();

            info.UserId = Convert.ToInt64(identity.Claims
                .Where(x => x.Type == "userId")
                .Select(s => s.Value)
                .FirstOrDefault());

            info.UserName = identity.Name;

            info.DefaultCurrency = identity.Claims
                .Where(x => x.Type == "defaultCurrency")
                .Select(s => s.Value)
                .FirstOrDefault();

            info.UserLevel = identity.Claims
                .Where(x => x.Type == "userLevel")
                .Select(s => s.Value)
                .FirstOrDefault();

            info.UserRole = identity.Claims
                .Where(x => x.Type == "userRole")
                .Select(s => s.Value)
                .FirstOrDefault();

            return info;
        }
    }
}