using Inventory360DataModel;
using BLL.Update.Task;
using System;
using System.Net;
using System.Security.Claims;
using System.Web.Http;

namespace Inventory360API_V2.Controllers
{
    [RoutePrefix("api")]
    public class TaskUpdateController : ApiController
    {
        [Authorize]
        [HttpPost]
        [Route("TE014")]
        public IHttpActionResult ApproveCollection(Guid id)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new UpdateTaskCollection()
                    .ApproveCollection(id, userInfo.CompanyId, userInfo.UserId);

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
        [Route("TU0408")]
        public IHttpActionResult ApproveTransferOrder(Guid id)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new UpdateTaskTransferOrder()
                    .ApproveTransferOrder(id, userInfo.CompanyId, userInfo.UserId);

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
        [Route("TU0409")]
        public IHttpActionResult CancelTransferOrder(Guid id,string reason)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new UpdateTaskTransferOrder()
                    .CancelTransferOrder(id, reason, userInfo.CompanyId, userInfo.UserId);

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
        [Route("TE013")]
        public IHttpActionResult CancelCollection(Guid id, string reason)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new UpdateTaskCollection()
                    .CancelCollection(id, reason, userInfo.CompanyId, userInfo.UserId);

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
        [Route("TE201")]
        public IHttpActionResult ApproveComplainReceive(Guid id)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new UpdateTaskComplainReceive()
                    .ApproveComplainReceive(id, userInfo.CompanyId, userInfo.UserId);

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
        [Route("TE202")]
        public IHttpActionResult CancelComplainReceive(Guid id, string reason)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new UpdateTaskComplainReceive()
                    .CancelComplainReceive(id, reason, userInfo.CompanyId, userInfo.UserId);

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
        [Route("TE203")]
        public IHttpActionResult CancelConvertion(Guid id, string reason)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new UpdateTaskConvertion()
                    .CancelConvertion(id, reason, userInfo.CompanyId, userInfo.UserId);

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
        [Route("TE204")]
        public IHttpActionResult ApproveConvertion(Guid id)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new UpdateTaskConvertion()
                    .ApproveConvertion(id, userInfo.CompanyId, userInfo.LocationId, userInfo.UserId);

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