using Inventory360DataModel;
using BLL;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Inventory360API_V2
{
    public class MyAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // before validate context need to set additional parameter to contex
            // https://stackoverflow.com/questions/31442364/owin-oauth-send-additional-parameters

            var companyId = context.Parameters.Where(f => f.Key == "companyId").Select(f => f.Value).SingleOrDefault()[0];
            context.OwinContext.Set<string>("CompanyId", companyId);

            var locationId = context.Parameters.Where(f => f.Key == "locationId").Select(f => f.Value).SingleOrDefault()[0];
            context.OwinContext.Set<string>("LocationId", locationId);

            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            long companyId = Convert.ToInt64(context.OwinContext.Get<string>("CompanyId"));
            long locationId = Convert.ToInt64(context.OwinContext.Get<string>("LocationId"));
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            ManagerSecurity managerSecurity = new ManagerSecurity();
            CommonSecurityLoginCredential checkCredential = managerSecurity.CheckSecurityUserAtLogin(companyId, locationId,context.UserName, context.Password);

            if (checkCredential.IsSuccess)
            {
                identity.AddClaim(new Claim("companyId", checkCredential.LoginCompanyId.ToString()));
                identity.AddClaim(new Claim("companyCode", checkCredential.LoginCompanyCode));
                identity.AddClaim(new Claim("companyName", checkCredential.LoginCompanyName));
                identity.AddClaim(new Claim("locationId", checkCredential.LoginLocationId.ToString()));
                identity.AddClaim(new Claim("locationCode", checkCredential.LoginLocationCode));
                identity.AddClaim(new Claim("locationName", checkCredential.LoginLocationName));
                identity.AddClaim(new Claim("defaultCurrency", checkCredential.DefaultCurrency));
                identity.AddClaim(new Claim("userId", checkCredential.LoginUserId.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Name, checkCredential.LoginUserName));
                identity.AddClaim(new Claim("firstLogin", checkCredential.IsFirstLogin));
                identity.AddClaim(new Claim("userLevel", checkCredential.LoginUserLevel));
                identity.AddClaim(new Claim("userRole", checkCredential.LoginUserRole));
                context.Validated(identity);
            }
            else
            {
                context.SetError("invalid_grant", "Provide username and password is invalid");
                return;
            }
        }
    }
}