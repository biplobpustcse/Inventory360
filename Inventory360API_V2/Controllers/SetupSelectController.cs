using Inventory360DataModel;
using BLL;
using BLL.Grid.Setup;
using System;
using System.Net;
using System.Security.Claims;
using System.Web.Http;

namespace Inventory360API_V2.Controllers
{
    [RoutePrefix("api")]
    public class SetupSelectController : ApiController
    {
        [Authorize]
        [HttpGet]
        [Route("S0079")]
        public IHttpActionResult SelectSupplierShortInfo(long id, string currency)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridSetupSupplier()
                    .SelectSupplierShortInfo(id, currency, userInfo.CompanyId);

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
        [Route("S0073")]
        public IHttpActionResult SelectCustomerShortInfoByCompanyIdAndCustomerId(long id)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridSetupCustomer()
                    .SelectCustomerShortInfoByCustomerId(id, userInfo.CompanyId);

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
        [Route("S0044T")]
        // Load measurement name
        public IHttpActionResult MeasurementNameList()
        {
            try
            {
                var data = new ManagerDefault()
                    .MeasurementNameLists();

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
        [Route("S0004")]
        public IHttpActionResult SelectLoginLocationByCompanyId()
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new ManagerSetupLocation()
                    .SelectLoginLocationByCompanyId(userInfo.CompanyId);

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
        [Route("S0005")]
        public IHttpActionResult SelectLocationByCompanyIdExceptOwnLocation()
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new ManagerSetupLocation()
                    .SelectLocationByCompanyIdExceptOwnLocation(userInfo.CompanyId, userInfo.LocationId);

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
        [Route("S0003")]
        public IHttpActionResult GetUserInfo()
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new
                {
                    userInfo.CompanyName,
                    userInfo.LocationName,
                    userInfo.LocationId,
                    userInfo.UserName,
                    userInfo.DefaultCurrency,
                    userInfo.UserLevel,
                    userInfo.UserRole
                };

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("S0002")]
        public IHttpActionResult SelectLoginLocationByCompanyId(long companyId)
        {
            var data = new ManagerSetupLocation()
                .SelectLoginLocationByCompanyId(companyId);

            return Ok(data);
        }

        [HttpGet]
        [Route("S0001")]
        public IHttpActionResult GetAllCompany()
        {
            var data = new ManagerSetupCompany().SelectAllCompany();

            return Ok(data);
        }
        [Authorize]
        [HttpGet]
        [Route("S201")]
        public IHttpActionResult SelectProblemLists(string query,int pageIndex,int pageSize)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridSetupProblemSetup()
                    .SelectProblemLists(query, userInfo.CompanyId,pageIndex,pageSize);

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
        [Route("S202")]
        public IHttpActionResult GetProblemForComplainReceive()
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridSetupProblemSetup()
                    .GetProblemForComplainReceive(userInfo.CompanyId);

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
        [Route("S203")]
        public IHttpActionResult GetAllCharge()
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridSetupCharge()
                    .GetAllCharge(userInfo.CompanyId);

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
        [Route("S204")]
        public IHttpActionResult SelectConvertionRatioLists(string query, int pageIndex, int pageSize)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridSetupConvertionRatio()
                    .SelectConvertionRatioLists(query, userInfo.CompanyId, pageIndex, pageSize);

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
        [Route("S205")]
        //Load Product Company for dropdown
        public IHttpActionResult SelectConvertionRatioByCompanyId()
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridSetupConvertionRatio()
                    .SelectConvertionRatioByCompanyId(userInfo.CompanyId);

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
        [Route("S206")]
        //Load Product Company for dropdown
        public IHttpActionResult SelectConvertionRatioById(string convertionRatioId)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridSetupConvertionRatio()
                    .SelectConvertionRatioById((string.IsNullOrEmpty(convertionRatioId) ? Guid.Empty : new Guid(convertionRatioId)),userInfo.CompanyId);

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
        [Route("S203E")]
        public IHttpActionResult GetRMAWiseAllCharge()
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridSetupCharge()
                    .GetRMAWiseAllCharge(userInfo.CompanyId);

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