using Inventory360DataModel;
using Inventory360DataModel.Setup;
using BLL.Insert.Setup;
using System;
using System.Net;
using System.Security.Claims;
using System.Web.Http;

namespace Inventory360API_V2.Controllers
{
    [RoutePrefix("api")]
    public class SetupInsertController : ApiController
    {
        private UserIdentityInfo GetUserInfoFromIdentity()
        {
            var identity = (ClaimsIdentity)User.Identity;
            return GetUserIdentityInfo.GetUserInfo(identity);
        }
        [Authorize]
        [HttpPost]
        [Route("SI201")]
        public IHttpActionResult InsertProblemSetup(CommonSetupProblemSetup entityList)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                entityList.EntryBy = userInfo.UserId;
                entityList.CompanyId = userInfo.CompanyId;

                var data = new InsertSetupProblemSetup()
                    .InsertProblemSetup(entityList);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Authorize]
        [HttpPost]
        [Route("SI202")]
        public IHttpActionResult InsertConvertionRatio(CommonSetupConvertionRatio entityList)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                entityList.EntryBy = userInfo.UserId;
                entityList.CompanyId = userInfo.CompanyId;

                var data = new InsertSetupConvertionRatio()
                    .InsertConvertionRatio(entityList);

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