using BLL.DropDown.Others;
using System;
using System.Net;
using System.Web.Http;

namespace Inventory360API_V2.Controllers
{
    [RoutePrefix("api")]
    public class OthersSelectController : ApiController
    {
        [Authorize]
        [HttpGet]
        [Route("OS003")]
        public IHttpActionResult SelectSalesAnalysisReportName()
        {
            try
            {
                var data = new DropDownOthersReport()
                    .SelectSalesAnalysisReportNameForDropdown();

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
        [Route("OS200")]
        public IHttpActionResult SelectComplainReceiveAnalysisReportName()
        {
            try
            {
                var data = new DropDownOthersReport()
                    .SelectComplainReceiveAnalysisReportName();

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
        [Route("OS201")]
        public IHttpActionResult SelectCustomerDeliveryAnalysisReportName()
        {
            try
            {
                var data = new DropDownOthersReport()
                    .SelectCustomerDeliveryAnalysisReportName();

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
        [Route("OS202")]
        public IHttpActionResult SelectReplacementClaimAnalysisReportName()
        {
            try
            {
                var data = new DropDownOthersReport()
                    .SelectReplacementClaimAnalysisReportName();

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
        [Route("OS203")]
        public IHttpActionResult SelectReplacementReceiveAnalysisReportName()
        {
            try
            {
                var data = new DropDownOthersReport()
                    .SelectReplacementReceiveAnalysisReportName();

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