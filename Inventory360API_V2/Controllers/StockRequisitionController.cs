using Inventory360DataModel;
using Inventory360DataModel.Task;
using BLL.Grid.Task;
using BLL.Insert.Task;
using BLL.Update.Task;
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
    public class StockRequisitionController : ApiController
    {
        [Authorize]
        [HttpPost]
        [Route("TI00400")]
        // Save Stock Requisition Finalize
        public IHttpActionResult SaveStockTransferRequisitionFinalize(CommonTransferRequisitionFinalize commonTransferRequisitionFinalize)
        {
            try
            {               
                var userInfo = GetUserInfoFromIdentity();
                commonTransferRequisitionFinalize.LocationId = userInfo.LocationId;
                commonTransferRequisitionFinalize.CompanyId = userInfo.CompanyId;
                commonTransferRequisitionFinalize.EntryBy = userInfo.UserId;

                var data = new InsertTaskTransferRequisitionFinalize()
                    .InsertTransferRequisitionFinalize(commonTransferRequisitionFinalize);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("TS0401")]
        public IHttpActionResult SelectUnApprovedTransferRequisitionFinalizeLists(string query, int pageIndex, int pageSize)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskTranReqFinalize()
                    .SelectUnApprovedTransferRequisitionFinalizeLists(query, userInfo.LocationId, userInfo.CompanyId, pageIndex, pageSize);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("TU0402")]
        public IHttpActionResult ApproveStockTransferRequisition(Guid id)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new UpdateTaskTransferRequisitionFinalize()
                    .ApproveTransferRequisitionFinalize(id, userInfo.CompanyId, userInfo.UserId);

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
        [Route("TU0403")]
        public IHttpActionResult CancelStockTransferRequisition(Guid id, string reason)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new UpdateTaskTransferRequisitionFinalize()
                    .CancelTransferRequisitionFinalize(id, reason, userInfo.CompanyId, userInfo.UserId);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("TS0404")]
        public IHttpActionResult SelectTransferRequisitionFinalizeDetail(Guid id)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskTranReqFinalizeDetail()
                    .SelectTransferRequisitionFinalizeDetail(userInfo.CompanyId, id);

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
        [Route("TS0405")]
        public IHttpActionResult SelectAllTransferRequisitionLists(string query, int pageIndex, int pageSize)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskTranReqFinalize()
                    .SelectTransferRequisitionFinalizeLists(query, userInfo.LocationId, userInfo.CompanyId, pageIndex, pageSize);

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
