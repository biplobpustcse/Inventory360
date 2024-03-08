using BLL.DropDown;
using System;
using System.Net;
using System.Web.Http;

namespace Inventory360API_V2.Controllers
{
    [RoutePrefix("api")]
    public class ConfigurationController : ApiController
    {
        [Authorize]
        [HttpGet]
        [Route("CON001")]
        //Load Operation Type by Company for dropdown
        public IHttpActionResult SelectOperationTypeForDropdown()
        {
            try
            {
                var data = new DropDownConfigurationOperationType()
                    .SelectOperationTypeForDropdown();

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