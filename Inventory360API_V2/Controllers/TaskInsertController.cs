using Inventory360DataModel;
using Inventory360DataModel.Task;
using BLL.Insert.Task;
using System;
using System.Net;
using System.Security.Claims;
using System.Web.Http;

namespace Inventory360API_V2.Controllers
{
    [RoutePrefix("api")]
    public class TaskInsertController : ApiController
    {
        [Authorize]
        [HttpPost]
        [Route("TI009")]
        // Save Sales Collection
        public IHttpActionResult InsertSalesCollection(CommonTaskCollection entity)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                entity.CompanyId = userInfo.CompanyId;
                entity.LocationId = userInfo.LocationId;
                entity.EntryBy = userInfo.UserId;

                var data = new InsertTaskCollection()
                    .InsertSalesCollection(entity);

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
        [Route("TI00204")]
        // Save ChequeTreatment
        public IHttpActionResult InsertChequeTreatment(CommonTaskChequeTreatment entityList)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new InsertTaskChequeTreatment().InsertChequeTreatment(entityList, userInfo.UserId);

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
        [Route("TI00408")]
        // Save Transfer Order
        public IHttpActionResult InsertTransferOrder(CommonTransferOrder entity)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                entity.CompanyId = userInfo.CompanyId;
                entity.LocationId = userInfo.LocationId;
                var data = new InsertTaskTransferOrder()
                    .InsertTransferOrder(entity, userInfo.UserId);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        //public IHttpActionResult InsertChequeTreatment(List<CommonTaskChequeTreatment> entityList)
        //{
        //    try
        //    {
        //        var userInfo = GetUserInfoFromIdentity();
        //        //foreach (CommonTaskChequeTreatment item in entityList)
        //        //{
        //        //    item.TreatmentId = Guid.NewGuid();
        //        //    item.EntryBy = userInfo.UserId;
        //        //    item.EntryDate = DateTime.Now;
        //        //}

        //        var data = new InsertTaskChequeTreatment().InsertChequeTreatment(entityList, userInfo.UserId);

        //        return Ok(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        //need to write error in txt file to fix the bug
        //        return Content(HttpStatusCode.BadRequest, ex.Message);
        //    }
        //}
        [Authorize]
        [HttpPost]
        [Route("TI00205")]
        // Save ChequeTreatment
        public IHttpActionResult InsertConvertion(CommonTaskConvertion entity)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                entity.CompanyId = userInfo.CompanyId;
                entity.LocationId = userInfo.LocationId;
                entity.EntryBy = userInfo.UserId;
                var data = new InsertTaskConvertion().InsertConvertion(entity);

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
