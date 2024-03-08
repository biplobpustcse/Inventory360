using Inventory360DataModel;
using BLL.DropDown;
using BLL.DropDown.Task;
using BLL.Grid.Task;
using System;
using System.Net;
using System.Security.Claims;
using System.Web.Http;

namespace Inventory360API_V2.Controllers
{
    [RoutePrefix("api")]
    public class TaskSelectController : ApiController
    {
        [Authorize]
        [HttpGet]
        [Route("TS00241")]
        public IHttpActionResult SelectAdvanceSearchDataLists(string serial)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskAdvanceSearch()
                    .SelectAdvanceSearchDataLists(serial,userInfo.CompanyId);

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
        [Route("TS00240")]
        public IHttpActionResult SelectAllReplacementClaimDetail_Problem(string claimDetailId)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskReplacementClaim()
                    .SelectAllReplacementClaimDetail_Problem(userInfo.CompanyId, (string.IsNullOrEmpty(claimDetailId) ? Guid.Empty : new Guid(claimDetailId)));

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
        [Route("TS00239")]
        //Load all Replacement Claim info by product and Company
        public IHttpActionResult SelectReplacementClaimInfoByProduct(long productId, string productSerial, string claimId)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskReplacementClaim()
                    .SelectReplacementClaimInfoByProduct(productId, productSerial, userInfo.CompanyId, (string.IsNullOrEmpty(claimId) ? Guid.Empty : new Guid(claimId)));
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
        [Route("TS00238")]
        //Load Product by serial from ComplainReceive Company for dropdown
        public IHttpActionResult SelectProductBySerialFromReplacementClaim(string serial, long productId, string claimId)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownReplacementClaim()
                    .SelectProductBySerialFromReplacementClaim(userInfo.CompanyId, userInfo.LocationId, serial, productId, (string.IsNullOrEmpty(claimId) ? Guid.Empty : new Guid(claimId)));

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
        [Route("TS00237")]
        //Load Product Company for dropdown
        public IHttpActionResult SelectProductNameByReplacementClaim(string query = "", string claimId = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownReplacementClaim()
                    .SelectProductNameByReplacementClaim(userInfo.CompanyId, query, (string.IsNullOrEmpty(claimId) ? Guid.Empty : new Guid(claimId)));

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
        [Route("TS0087")]
        //Load all Invoice No by Company for dropdown
        public IHttpActionResult SelectReplacementClaimShortInfoById(string id = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskReplacementClaim()
                    .SelectReplacementClaimShortInfoById((string.IsNullOrEmpty(id) ? Guid.Empty : new Guid(id)), userInfo.CompanyId);

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
        [Route("TS0086")]
        //Load all Complain Receive No by Company for dropdown
        public IHttpActionResult SelectAllReplacementClaimNoByCompanyIdForDropdown(string query, long supplierId, DateTime? dateFrom, DateTime? dateTo)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownReplacementClaim()
                    .SelectAllReplacementClaimNoByCompanyIdForDropdown(query, supplierId, dateFrom, dateTo, userInfo.CompanyId, userInfo.LocationId);

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
        [Route("TS0028")]
        public IHttpActionResult SelectItemRequisitionWithDetailForFinalize(string query)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskItemRequisitionDetail()
                    .SelectItemRequisitionWithDetailForFinalize(query, userInfo.CompanyId);

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
        [Route("TS0023")]
        public IHttpActionResult SelectAllCollectionLists(string query, string currency, int pageIndex, int pageSize)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskCollection()
                    .SelectAllCollectionLists(query, currency, userInfo.CompanyId, pageIndex, pageSize);

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
        [Route("TS0022")]
        public IHttpActionResult SelectUnApprovedCollectionLists(string query, string currency, int pageIndex, int pageSize)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskCollection()
                    .SelectUnApprovedCollectionLists(query, currency, userInfo.CompanyId, pageIndex, pageSize);

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
        [Route("TS0021")]
        public IHttpActionResult SelectCollectionMappingDataByCustomerIdAndCollectionAgainst(string collectionAgainst, string currency, long customerId)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskCollection()
                    .SelectCollectionMappingDataByCustomerIdAndCollectionAgainst(collectionAgainst, currency, customerId, userInfo.CompanyId);

                return Ok(data);
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        //Added By Biplob 09/03/2020       
        [Authorize]
        [HttpGet]
        [Route("TS00203")]
        public IHttpActionResult SelectAllChequeInfoLists( string searchQuery,string chequeTypValue, DateTime? dateFrom, DateTime? dateTo,string chequeStatusCode,long selectedLocationId,long ownBankId,long CustomerOrSupplierId, string currency, int pageIndex, int pageSize)
        {
            try
            {
               // string chequeStatusCode = ""; long locationId = 0; long ownBankId =0;
                //chequeStatusCode, locationId, ownBankId,dateFrom, dateTo,  
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskChequeTreatement()
                    .SelectAllChequeInfoLists(searchQuery, chequeTypValue, dateFrom, dateTo, chequeStatusCode, selectedLocationId, ownBankId, CustomerOrSupplierId, currency, userInfo.CompanyId, pageIndex, pageSize);

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
        [Route("TS00204")]
        //Load Project
        public IHttpActionResult SelectChequeNo(string query)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownChequeNo()
                    .SelectChequeNo(query,userInfo.CompanyId, userInfo.LocationId);

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
        [Route("TS0407")]
        public IHttpActionResult SelectTransferRequisitionWithDetailForTransferOrder(string query)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskTranReqFinalize()
                    .SelectTransferRequisitionFinalizeDetailForTransferOrder(query,userInfo.LocationId, userInfo.CompanyId);//, pageIndex, pageSize

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
        [Route("TS0408")]
        public IHttpActionResult SelectAllTransferOrder(string query, int pageIndex, int pageSize)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskTransferOrder()
                    .SelectAllTransferOrder(query, userInfo.LocationId, userInfo.CompanyId, pageIndex, pageSize);

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
        [Route("TS0409")]
        public IHttpActionResult SelectUnApprovedTransferOrderLists(string query, int pageIndex, int pageSize)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskTransferOrder()
                    .SelectUnApprovedTransferOrderLists(query, userInfo.LocationId, userInfo.CompanyId, pageIndex, pageSize);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("TS0410")]
        public IHttpActionResult SelectTransferOrderDetail(Guid id)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskTransferOrderDetail()
                    .SelectTransferOrderDetail(userInfo.CompanyId, id);

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
        [Route("TS0411")]
        public IHttpActionResult SelectTransferOrderSearch(long transferTo,string fromDate,string toDate,string fromStockType,string toStockType)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskTransferOrder().SelectApprovedTransferOrderLists(userInfo.CompanyId, userInfo.LocationId, transferTo, fromDate, toDate, fromStockType, toStockType);
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
        [Route("TS0412")]
        public IHttpActionResult SelectProductWarehouseByLocation(long productid, string orderid)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskTransferOrder().SelectProductWarehouseByLocation(userInfo.CompanyId, userInfo.LocationId, productid, orderid);
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
        [Route("TS0413")]
        public IHttpActionResult SelectProductSerialNos(long productid, string orderId)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskTransferOrder().SelectProductSerialNos(userInfo.CompanyId, userInfo.LocationId, productid, orderId);
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
        [Route("TS0414")]
        public IHttpActionResult SelectProductDetailInfo(long productid, string orderId)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskTransferOrder().SelectProductDetailInfo(userInfo.CompanyId, userInfo.LocationId, productid, orderId);
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
        [Route("TS0415")]
        public IHttpActionResult SelectProductWarehouseByLocationForSerialProduct(long productid, string orderId)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskTransferOrder().SelectProductWarehouseByLocationForSerialProduct(userInfo.CompanyId, userInfo.LocationId, productid, orderId);
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
        [Route("TS0416")]
        public IHttpActionResult SelectWarehouseBasedSerialNo(long productid, long warehouseId, string orderId)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskTransferOrder().SelectWarehouseBasedSerialNo(userInfo.CompanyId, userInfo.LocationId, productid, warehouseId, orderId);
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
        [Route("TS00205")]
        public IHttpActionResult SelectComplainReceiveLists(string query, int pageIndex, int pageSize)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskComplainReceive()
                   .SelectComplainReceiveLists(query, userInfo.LocationId, userInfo.CompanyId, pageIndex, pageSize);
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
        [Route("TS00206")]
        public IHttpActionResult SelectCustomerDeliveryLists(string query, int pageIndex, int pageSize)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskCustomerDelivery()
                   .SelectCustomerDeliveryLists(query, userInfo.LocationId, userInfo.CompanyId, pageIndex, pageSize);
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
        [Route("TS00207")]
        //Load all Complain Receive No by Company for dropdown
        public IHttpActionResult SelectAllComplainReceiveNoByCompanyIdForDropdown(string query, long CustomerId, string dateFrom, string dateTo)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownComplainReceive()
                    .SelectAllComplainReceiveNoByCompanyIdForDropdown(query, CustomerId, dateFrom, dateTo, userInfo.CompanyId, userInfo.LocationId);

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
        [Route("TS00208")]
        //Load Product Company for dropdown
        public IHttpActionResult SelectProductNameByComplainReceive(string query = "", string ComplainReceiveId = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownComplainReceive()
                    .SelectProductNameByComplainReceive(userInfo.CompanyId, query, (string.IsNullOrEmpty(ComplainReceiveId) ? Guid.Empty : new Guid(ComplainReceiveId)));

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
        [Route("TS00209")]
        //Load Product by serial from ComplainReceive Company for dropdown
        public IHttpActionResult SelectProductBySerialFromComplainReceive(string serial, long ProductId, string complainReceiveId)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownComplainReceive()
                    .SelectProductBySerialFromComplainReceive(userInfo.CompanyId, userInfo.LocationId, serial, ProductId, (string.IsNullOrEmpty(complainReceiveId) ? Guid.Empty : new Guid(complainReceiveId)));

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
        [Route("TS00210")]
        public IHttpActionResult GetAllProblemWithComplainReceived(string ReceiveDetailId)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskComplainReceive()
                    .GetAllProblemWithComplainReceivedForCustomerDelivery(userInfo.CompanyId, (string.IsNullOrEmpty(ReceiveDetailId) ? Guid.Empty : new Guid(ReceiveDetailId)));

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
        [Route("TS00211")]
        public IHttpActionResult GetAllChargeWithComplainReceived(string ComplainReceiveId, string currency)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskComplainReceive()
                    .GetAllChargeWithComplainReceived(userInfo.CompanyId, (string.IsNullOrEmpty(ComplainReceiveId) ? Guid.Empty : new Guid(ComplainReceiveId)), currency);

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
        [Route("TS00212")]
        public IHttpActionResult GetSpareProductByParentRcvdIdAndProduct(string ComplainReceiveId,long ProductId, string Serial)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskComplainReceive()
                    .GetSpareProductByParentRcvdIdAndProduct(userInfo.CompanyId, (string.IsNullOrEmpty(ComplainReceiveId) ? Guid.Empty : new Guid(ComplainReceiveId)), ProductId, Serial);

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
        [Route("TS002013")]
        public IHttpActionResult SelectUnApprovedComplainReceiveLists(string query, int pageIndex, int pageSize)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskComplainReceive()
                   .SelectUnApprovedComplainReceiveLists(query, userInfo.LocationId, userInfo.CompanyId, pageIndex, pageSize);
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
        [Route("TS00214")]
        //Load RMA Product by serial from ComplainReceive Company for dropdown
        public IHttpActionResult SelectRMAProductBySerialFromComplainReceive(string serial, long ProductId, string complainReceiveId)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownComplainReceive()
                    .SelectRMAProductBySerialFromComplainReceive(userInfo.CompanyId, userInfo.LocationId, serial, ProductId, (string.IsNullOrEmpty(complainReceiveId) ? Guid.Empty : new Guid(complainReceiveId)));

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
        [Route("TS00215")]
        public IHttpActionResult SelectReplacementClaimLists(string query, int pageIndex, int pageSize)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskReplacementClaim()
                   .SelectReplacementClaimLists(query, userInfo.LocationId, userInfo.CompanyId, pageIndex, pageSize);
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
        [Route("TS00216")]
        //Load Product Company for dropdown
        public IHttpActionResult SelectRMAProductNameByComplainReceive(string query = "", string ComplainReceiveId = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new DropDownComplainReceive()
                    .SelectRMAProductNameByComplainReceive(userInfo.CompanyId, query, (string.IsNullOrEmpty(ComplainReceiveId) ? Guid.Empty : new Guid(ComplainReceiveId)));

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
        [Route("TS00217")]
        public IHttpActionResult SelectReplacementReceiveLists(string query, int pageIndex, int pageSize)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskReplacementReceive()
                   .SelectReplacementReceiveLists(query, userInfo.LocationId, userInfo.CompanyId, pageIndex, pageSize);
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
        [Route("TS00218")]
        public IHttpActionResult GetAllProblemWithComplainReceivedForReplacementClaim(string ReceiveDetailId)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskComplainReceive()
                    .GetAllProblemWithComplainReceivedForReplacementClaim(userInfo.CompanyId, (string.IsNullOrEmpty(ReceiveDetailId) ? Guid.Empty : new Guid(ReceiveDetailId)));

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
        [Route("TS00219")]
        public IHttpActionResult SelectConvertionLists(string query, int pageIndex, int pageSize)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskConvertion()
                   .SelectConvertionList(query, userInfo.LocationId, userInfo.CompanyId, pageIndex, pageSize);
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
        [Route("TS00220")]
        public IHttpActionResult SelectUnApprovedConvertionLists(string query, int pageIndex, int pageSize)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridTaskConvertion()
                   .SelectUnApprovedConvertionLists(query, userInfo.LocationId, userInfo.CompanyId, pageIndex, pageSize);
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