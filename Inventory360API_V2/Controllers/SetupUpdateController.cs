using Inventory360DataModel;
using Inventory360DataModel.Setup;
using BLL.Update;
using BLL.Update.Setup;
using System;
using System.Net;
using System.Security.Claims;
using System.Web.Http;

namespace Inventory360API_V2.Controllers
{
    [RoutePrefix("api")]
    public class SetupUpdateController : ApiController
    {
        [Authorize]
        [HttpGet]
        [Route("UpdateDatabase")]
        public IHttpActionResult UpdateDatabase()
        {
            try
            {
                var data = new UpdateDatabaseUpdate()
                    .UpdateDatabase();

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
        [Route("SU201")]
        public IHttpActionResult UpdateProblemSetup(CommonSetupProblemSetup entity)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                entity.EntryBy = userInfo.UserId;
                entity.CompanyId = userInfo.CompanyId;

                var data = new UpdateSetupProblemSetup()
                    .UpdateProblemSetup(entity);

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
        [Route("SU202")]
        public IHttpActionResult UpdateConvertionRatioSetup(CommonSetupConvertionRatio entity)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                entity.EntryBy = userInfo.UserId;
                entity.CompanyId = userInfo.CompanyId;

                var data = new UpdateSetupConvertionRatio()
                    .UpdateConvertionRatioSetup(entity);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        private UserIdentityInfo GetUserInfoFromIdentity()
        {
            var identity = (ClaimsIdentity)User.Identity;
            return GetUserIdentityInfo.GetUserInfo(identity);
        }
    }
}