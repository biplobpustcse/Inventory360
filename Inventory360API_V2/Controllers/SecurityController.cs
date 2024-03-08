using Inventory360DataModel;
using BLL;
using System;
using System.Net;
using System.Security.Claims;
using System.Web.Http;

namespace Inventory360API_V2.Controllers
{
    [RoutePrefix("api")]
    public class SecurityController : ApiController
    {
        [Authorize]
        [HttpPost]
        [Route("SEC02")]
        public IHttpActionResult ChangeSecurityUserPassword(ChangeSecurityPassword entity)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var userInfo = GetUserIdentityInfo.GetUserInfo(identity);

                var data = new ManagerSecurity()
                    .ChangeSecurityUserPassword(userInfo.CompanyId, userInfo.LocationId, userInfo.UserId, entity.CurrentPassword, entity.NewPassword, entity.ConfirmPassword);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
