using Inventory360DataModel;
using BLL.Grid.Report;
using BLL.Grid.Task;
using System;
using System.Net;
using System.Security.Claims;
using System.Web.Http;

namespace Inventory360API_V2.Controllers
{
    [RoutePrefix("api")]
    public class ReportsController : ApiController
    {
        [Authorize]
        [HttpGet]
        [Route("R0009")]
        public IHttpActionResult GenerateCollectionReport(string currency, string collectionNo, string collectionId = "")
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridReportCollection()
                    .GenerateCollectionReport(userInfo.CompanyId, currency, collectionNo, (string.IsNullOrEmpty(collectionId) ? Guid.Empty : new Guid(collectionId)));

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
        [Route("R0200")]
        public IHttpActionResult GenerateChequeTreatementStatusWiseChequeDetailReport(string ReportName, string dataPositionOptionValue, string chequeType, long LocationId, long bankId, long customerOrSupplierId, DateTime? dateFrom, DateTime? dateTo, string chequeStatusCode, string chequeOrTreatementBankOptionValue, string chequeCollectionOrPaymentDateOptionValue)
        {
            try
            {
                string currency = "BDT";

                var userInfo = GetUserInfoFromIdentity();
                if(ReportName == "ChequeInHand" || ReportName == "AdvanceChequeIssued")
                {
                    var data = new GridReportChequeTreatement()
                     .GenerateChequeTreatementStatusWiseChequeDetailReportChequeInHand(userInfo.CompanyId, currency, ReportName, dataPositionOptionValue, chequeType, LocationId, bankId, customerOrSupplierId, dateFrom, dateTo, chequeStatusCode, chequeOrTreatementBankOptionValue, chequeCollectionOrPaymentDateOptionValue);
                    return Ok(data);
                }
                else if (ReportName == "ChequeHistory")
                {
                    var data = new GridReportChequeTreatement()
                     .GenerateChequeTreatementStatusWiseChequeDetailReportChequeHistory(userInfo.CompanyId, currency, ReportName, dataPositionOptionValue, chequeType, LocationId, bankId, customerOrSupplierId, dateFrom, dateTo, chequeStatusCode, chequeOrTreatementBankOptionValue, chequeCollectionOrPaymentDateOptionValue);
                    return Ok(data);
                }
                else if(ReportName == "CustomerSupplierwiseChequePerformance")
                {            
                    var data = new GridReportChequeTreatement()
                     .GenerateChequeTreatementStatusWiseChequeDetailReportCustomerSupplierwiseChequePerformance(userInfo.CompanyId,userInfo.UserId, currency, ReportName, dataPositionOptionValue, chequeType, LocationId, bankId, customerOrSupplierId, dateFrom, dateTo, chequeStatusCode, chequeOrTreatementBankOptionValue, chequeCollectionOrPaymentDateOptionValue);
                    return Ok(data);
                }
                else
                {
                var data = new GridReportChequeTreatement()
                    .GenerateChequeTreatementStatusWiseChequeDetailReport(userInfo.CompanyId, currency, ReportName, dataPositionOptionValue, chequeType, LocationId, bankId, customerOrSupplierId,  dateFrom, dateTo, chequeStatusCode, chequeOrTreatementBankOptionValue, chequeCollectionOrPaymentDateOptionValue);
                    return Ok(data);
                }
                
            }
            catch (Exception ex)
            {
                //need to write error in txt file to fix the bug
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("R0409")]
        public IHttpActionResult GenerateTransferRequisitionReport(string TransferRequisitionId, string TransferRequisitionNo)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridReportTransferRequisition()
                    .GenerateTransferRequisitionReport(userInfo.CompanyId, TransferRequisitionNo, (string.IsNullOrEmpty(TransferRequisitionId) ? Guid.Empty : new Guid(TransferRequisitionId)));

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
        [Route("R0410")]
        public IHttpActionResult GenerateTransferOrderReport(string TransferOrderId,string TransferOrderNo)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridReportTransferOrder()
                    .GenerateTransferOrderReport(userInfo.CompanyId, userInfo.LocationId,TransferOrderId, TransferOrderNo);

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
        [Route("R0201")]
        public IHttpActionResult GenerateComplainReceiveReport(string ReceiveId, string ReceiveNo)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridReportComplainReceive()
                    .GenerateComplainReceiveReport(userInfo.CompanyId, userInfo.LocationId, ReceiveId, ReceiveNo);

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
        [Route("R0202")]
        public IHttpActionResult GenerateCustomerDeliveryReport(string DeliveryId, string DeliveryNo)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridReportCustomerDelivery()
                    .GenerateCustomerDeliveryReport(userInfo.CompanyId, userInfo.LocationId, DeliveryId, DeliveryNo);

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
        [Route("R0203")]
        public IHttpActionResult GenerateReplacementClaimReport(string ClaimId, string ClaimNo)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridReportReplacementClaim()
                    .GenerateReplacementClaim(userInfo.CompanyId, userInfo.LocationId, ClaimId, ClaimNo);

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
        [Route("R0204")]
        public IHttpActionResult GenerateReplacementReceiveReport(string ReceiveId, string ReceiveNo)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridReportReplacementReceive()
                    .GenerateReplacementReceive(userInfo.CompanyId, userInfo.LocationId, ReceiveId, ReceiveNo);

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
        [Route("R0029")]
        public IHttpActionResult GenerateSalesAnalysisReport(long locationId, string dateFrom, string dateTo, long salesPersonId, string salesMode, long customerGroupId, long customerId, long groupId, long subGroupId, long categoryId, long brandId, string model, long productId, string currencyId)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridReportSalesAnalysisReport()
                    .GenerateSalesAnalysisReport(locationId, dateFrom, dateTo, salesPersonId, salesMode, customerGroupId, customerId, groupId, subGroupId, categoryId, brandId, model, productId, currencyId, userInfo.CompanyId);

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
        [Route("R0205")]
        public IHttpActionResult GenerateComplainReceiveAnalysisReport(long locationId, string dateFrom, string dateTo, string complainReceiveId, long customerGroupId, long customerId, long groupId, long subGroupId, long categoryId, long brandId, string model, long productId, string currencyId)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridReportComplainReceiveAnalysisReport()
                    .GenerateComplainReceiveAnalysisReport(locationId, dateFrom, dateTo, (string.IsNullOrEmpty(complainReceiveId) ? Guid.Empty : new Guid(complainReceiveId)), customerGroupId, customerId, groupId, subGroupId, categoryId, brandId, model, productId, currencyId, userInfo.CompanyId);

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
        [Route("R0206")]
        public IHttpActionResult GenerateCustomerDeliveryAnalysisReport(long locationId, string dateFrom, string dateTo, string customerDeliveryId, long customerGroupId, long customerId, long groupId, long subGroupId, long categoryId, long brandId, string model, long productId, string currencyId)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridReportCustomerDeliveryAnalysisReport()
                    .GenerateCustomerDeliveryAnalysisReport(locationId, dateFrom, dateTo, (string.IsNullOrEmpty(customerDeliveryId) ? Guid.Empty : new Guid(customerDeliveryId)), customerGroupId, customerId, groupId, subGroupId, categoryId, brandId, model, productId, currencyId, userInfo.CompanyId);

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
        [Route("R0207")]
        public IHttpActionResult GenerateReplacementClaimAnalysisReport(long locationId, string dateFrom, string dateTo,  long supplierGroupId, long supplierId, long groupId, long subGroupId, long categoryId, long brandId, string model, long productId, string currencyId)
        {
            try
            {
                string claimId = "";
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridReportReplacementClaimAnalysis()
                    .GenerateReplacementClaimAnalysisReport(locationId, dateFrom, dateTo, (string.IsNullOrEmpty(claimId) ? Guid.Empty : new Guid(claimId)), supplierGroupId, supplierId, groupId, subGroupId, categoryId, brandId, model, productId, currencyId, userInfo.CompanyId);

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
        [Route("R0208")]
        public IHttpActionResult GenerateReplacementReceiveAnalysisReport(long locationId, string dateFrom, string dateTo, string replacementReceiveId, long supplierGroupId, long supplierId, long groupId, long subGroupId, long categoryId, long brandId, string model, long productId, string currencyId)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridReportReplacementReceiveAnalysis()
                    .GenerateReplacementReceiveAnalysisReport(locationId, dateFrom, dateTo, (string.IsNullOrEmpty(replacementReceiveId) ? Guid.Empty : new Guid(replacementReceiveId)), supplierGroupId, supplierId, groupId, subGroupId, categoryId, brandId, model, productId, currencyId, userInfo.CompanyId);

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
        [Route("R0209")]
        public IHttpActionResult GenerateConvertionReport(string ConvertionId, string ConvertionNo)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridReportConvertion()
                    .GenerateConvertionReport(userInfo.CompanyId, userInfo.LocationId, ConvertionId, ConvertionNo);

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
        [Route("R0210")]
        public IHttpActionResult GenerateConvertionRatioReport(string ConvertionRatioId, string RatioNo)
        {
            try
            {
                var userInfo = GetUserInfoFromIdentity();
                var data = new GridReportConvertionRatio()
                    .GenerateConvertionRatioReport(userInfo.CompanyId, userInfo.LocationId, ConvertionRatioId, RatioNo);

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
