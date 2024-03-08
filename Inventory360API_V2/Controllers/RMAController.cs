using BLL.Grid.Task;
using BLL.Insert.Task;
using BLL.Update.Task;
using Inventory360DataModel;
using Inventory360DataModel.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace Inventory360API_V2.Controllers
{
    [RoutePrefix("api")]
    public class RMAController : ApiController
    {
        [Authorize]
        [HttpPost]
        [Route("TI00200")]
        // Save Complain Receive
        public IHttpActionResult SaveComplainReceive(CommonComplainReceive commonComplainReceive)
        {
            try
            {               
                var userInfo = GetUserInfoFromIdentity();
                commonComplainReceive.LocationId = userInfo.LocationId;
                commonComplainReceive.CompanyId = userInfo.CompanyId;
                commonComplainReceive.EntryBy = userInfo.UserId;

                var data = new InsertTaskComplainReceive()
                    .InsertComplainReceive(commonComplainReceive);

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
        [Route("TI00201")]
        // Save Complain Receive
        public IHttpActionResult SaveCustomerDelivery(CommonTaskCustomerDelivery commonTaskCustomerDelivery)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                commonTaskCustomerDelivery.LocationId = userInfo.LocationId;
                commonTaskCustomerDelivery.CompanyId = userInfo.CompanyId;
                commonTaskCustomerDelivery.EntryBy = userInfo.UserId;

                var data = new InsertTaskCustomerDelivery()
                    .InsertCustomerDelivery(commonTaskCustomerDelivery);

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
        [Route("TI00202")]
        //Save Replacement Claim
        public IHttpActionResult SaveReplacementClaim(CommonReplacementClaim commonReplacementClaim)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                commonReplacementClaim.LocationId = userInfo.LocationId;
                commonReplacementClaim.CompanyId = userInfo.CompanyId;
                commonReplacementClaim.EntryBy = userInfo.UserId;

                var data = new InsertTaskReplacementClaim()
                    .InsertReplacementClaim(commonReplacementClaim);

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
        [Route("TI00203")]
        // Save Complain Receive
        public IHttpActionResult SaveReplacementReceive(CommonTaskReplacementReceive entity)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                entity.LocationId = userInfo.LocationId;
                entity.CompanyId = userInfo.CompanyId;
                entity.EntryBy = userInfo.UserId;

                var data = new InsertTaskReplacementReceive()
                    .InsertReplacementReceive(entity);

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
