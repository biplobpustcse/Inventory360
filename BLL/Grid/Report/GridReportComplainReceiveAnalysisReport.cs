using Inventory360DataModel;
using BLL.Common;
using DAL.DataAccess.Select.Setup;
using DAL.DataAccess.Select.Task;
using DAL.Interface.Select.Setup;
using DAL.Interface.Select.Task;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Grid.Report
{
    public class GridReportComplainReceiveAnalysisReport
    {
        public object GenerateComplainReceiveAnalysisReport(long locationId, string dateFrom, string dateTo, Guid complainReceiveId, long customerGroupId, long customerId, long groupId, long subGroupId, long categoryId, long brandId, string model, long productId, string currency, long companyId)
        {
            try
            {

                DateTime? fromDate = MyConversion.ConvertDateStringToDate(dateFrom);
                DateTime? toDate = MyConversion.ConvertDateStringToDate(dateTo);

                var currencyInfo = GetCompanyCurrencyInfo.CompanyCurrencyInfo(companyId);
                ISelectSetupCompany iSelectSetupCompany = new DSelectSetupCompany();
                var companyInfo = iSelectSetupCompany.SelectCompanyAll()
                    .Where(x => x.CompanyId == companyId)
                    .Select(s => new
                    {
                        CompanyName = s.Name,
                        CompanyAddress = s.Address,
                        Phone = s.PhoneNo,
                        Fax = s.FaxNo
                    })
                    .FirstOrDefault();

                ISelectTaskComplainReceiveDetail iSelectTaskComplainReceiveDetail = new DSelectTaskComplainReceiveDetail(companyId);
                var complainReceiveInfo = iSelectTaskComplainReceiveDetail.SelectTaskComplainReceiveDetailAll()
                    .WhereIf(locationId > 0, x => x.Task_ComplainReceive.LocationId == locationId)
                    .WhereIf(locationId > 0, x => x.Task_ComplainReceive.Approved == "A")
                    .WhereIf(fromDate != null, x => DbFunctions.TruncateTime(x.Task_ComplainReceive.ReceiveDate) >= DbFunctions.TruncateTime(fromDate))
                    .WhereIf(toDate != null, x => DbFunctions.TruncateTime(x.Task_ComplainReceive.ReceiveDate) <= DbFunctions.TruncateTime(toDate))
                    .WhereIf(customerGroupId > 0, x => x.Task_ComplainReceive.Setup_Customer.CustomerGroupId == customerGroupId)
                    .WhereIf(customerId > 0, x => x.Task_ComplainReceive.CustomerId == customerId)
                    .WhereIf(groupId > 0, x => x.Setup_Product.ProductGroupId == groupId)
                    .WhereIf(subGroupId > 0, x => x.Setup_Product.ProductSubGroupId == subGroupId)
                    .WhereIf(categoryId > 0, x => x.Setup_Product.ProductCategoryId == categoryId)
                    .WhereIf(brandId > 0, x => x.Setup_Product.BrandId == brandId)
                    .WhereIf(!string.IsNullOrEmpty(model), x => x.Setup_Product.Model.ToLower().Contains(model.ToLower()))
                    .WhereIf(productId > 0, x => x.ProductId == productId)
                    .Select(s => new
                    {
                        Location = s.Task_ComplainReceive.Setup_Location.Name,
                        CustomerGroup = s.Task_ComplainReceive.Setup_Customer.Setup_CustomerGroup.Name,
                        s.Task_ComplainReceive.CustomerId,
                        s.Task_ComplainReceive.Setup_Customer.SalesPersonId,
                        s.Task_ComplainReceive.Setup_Customer.PhoneNo,
                        s.ReceiveId,
                        s.Task_ComplainReceive.ReceiveNo,
                        s.Task_SalesInvoice.InvoiceNo,
                        ReceiveDate = (DateTime)DbFunctions.TruncateTime(s.Task_ComplainReceive.ReceiveDate),
                        ReceivedBy = s.Task_ComplainReceive.Setup_Employee.Name,
                        ReceivedByContactNo = s.Task_ComplainReceive.Setup_Employee.ContactNo,
                        ApprovedBy = s.Task_ComplainReceive.Security_User.UserName,
                        EntryBy = s.Task_ComplainReceive.Security_User1.UserName,
                        ProductGroup = s.Setup_Product.Setup_ProductGroup.Name,
                        s.Setup_Product.Model,
                        ProductCode = s.Setup_Product.Code,
                        ProductName = s.Setup_Product.Name,
                        ProductId = s.ProductId,
                        s.Serial,
                        s.Remarks,
                        s.ProductDimensionId,
                        UnitType = s.Setup_UnitType.Name,
                        ComplainReceiveDetail_Problem = s.Task_ComplainReceiveDetail_Problem.Select(x => new Problem { Name = x.Setup_Problem.Name }).ToList()
                    })
                    .ToList()
                    .Select(s => new ComplainReceiveDetailInfoForAnalysis(s.CustomerId, s.ProductDimensionId, companyId)
                    {
                        Location = s.Location,
                        CustomerGroup = s.CustomerGroup,
                        ReceiveId = s.ReceiveId,
                        ReceiveNo = s.ReceiveNo,
                        InvoiceNo = s.InvoiceNo,
                        ReceiveDate = s.ReceiveDate,
                        CustomerPhoneNo = s.PhoneNo,
                        ReceivedBy = s.ReceivedBy,
                        ReceivedByContactNo = s.ReceivedByContactNo,
                        ApprovedBy = s.ApprovedBy,
                        EntryBy = s.EntryBy,
                        ProductGroup = s.ProductGroup,
                        Model = s.Model,
                        ProductCode = s.ProductCode,
                        ProductName = s.ProductName,
                        ProductId = s.ProductId,
                        Serial = s.Serial,
                        Remarks = s.Remarks,
                        UnitType = s.UnitType,
                        ProblemNames = s.ComplainReceiveDetail_Problem                        
                    }).ToList();
                //ReceiveStatus
                ComplainReceiveStatus(complainReceiveInfo, companyId);
                
                return new
                {
                    companyInfo.CompanyName,
                    companyInfo.CompanyAddress,
                    companyInfo.Phone,
                    companyInfo.Fax,
                    ComplainReceiveAnalysisLists = complainReceiveInfo
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ComplainReceiveStatus(List<ComplainReceiveDetailInfoForAnalysis> complainReceiveInfoList, long companyId)
        {
            var receiveIdIds = complainReceiveInfoList.Select(s => s.ReceiveId).Distinct().ToList();
            //CustomerDelivery
            ISelectTaskCustomerDeliveryDetail iSelectTaskCustomerDeliveryDetail = new DSelectTaskCustomerDeliveryDetail(companyId);
            var DeliveryInfoList = iSelectTaskCustomerDeliveryDetail.SelectCustomerDeliveryDetailAll()
                        .Where(x => receiveIdIds.Contains((Guid)x.ComplainReceiveId))
                        .Select(
                            y => new {
                                y.ComplainReceiveId,
                                y.Task_CustomerDelivery.DeliveryDate,
                                y.Task_CustomerDelivery.DeliveryNo,
                                DeliveredProductName = y.Setup_Product1.Name,
                                y.NewProductId,
                                y.NewSerial

                            }
                        ).ToList();
            //ReplacementClaim
            ISelectTaskReplacementClaimDetail iSelectTaskReplacementClaimDetail = new DSelectTaskReplacementClaimDetail(companyId);
            var replacementClaimInfoList = iSelectTaskReplacementClaimDetail.SelectReplacementClaimDetailAll()
                    .Where(x => receiveIdIds.Contains((Guid)x.ComplainReceiveId))
                    .Select(
                        y => new
                        {
                            y.ComplainReceiveId,
                            y.Task_ReplacementClaim.ClaimId,
                            y.Task_ReplacementClaim.ClaimNo,
                            y.ProductId
                        }
                    ).ToList();
            //ReplacementReceive
            var claimIdIds = replacementClaimInfoList.Select(s => s.ClaimId).Distinct().ToList();
            ISelectTaskReplacementReceiveDetail iSelectTaskReplacementReceiveDetail = new DSelectTaskReplacementReceiveDetail(companyId);
            var replacementReceiveInfoList = iSelectTaskReplacementReceiveDetail.SelectReplacementReceiveDetailAll()
                    .Where(x => claimIdIds.Contains((Guid)x.ReplacementClaimId))
                    .Select(
                        y => new
                        {
                            y.Task_ReplacementReceive.ReceiveId,
                            y.Task_ReplacementReceive.ReceiveNo,
                            y.ReplacementClaimId,
                            y.NewProductId
                        }
                    ).ToList();
            foreach (ComplainReceiveDetailInfoForAnalysis complainReceiveInfo in complainReceiveInfoList)
            {
                var DeliveryInfo = DeliveryInfoList.Where(x=>x.ComplainReceiveId == complainReceiveInfo.ReceiveId && x.NewProductId == complainReceiveInfo.ProductId).FirstOrDefault();
                if (DeliveryInfo != null)
                {
                    complainReceiveInfo.Status = "Delivered";
                    complainReceiveInfo.DeliveryStatus = "Y";
                    complainReceiveInfo.DeliveryNumber = DeliveryInfo.DeliveryNo;
                    complainReceiveInfo.DeliveryDate = DeliveryInfo.DeliveryDate;
                    complainReceiveInfo.DeliveredProductName = DeliveryInfo.DeliveredProductName;
                    complainReceiveInfo.DeliveredSerial = DeliveryInfo.NewSerial;
                    complainReceiveInfo.DeliveryDaysTaken = (int)(DeliveryInfo.DeliveryDate - complainReceiveInfo.ReceiveDate).TotalDays;
                    complainReceiveInfo.SettlementType = "";
                }
                else
                {
                    complainReceiveInfo.DeliveryStatus = "N";
                    complainReceiveInfo.DeliveryDaysTaken = (int)(DateTime.Now - complainReceiveInfo.ReceiveDate).TotalDays;

                    var replacementClaimInfo = replacementClaimInfoList.Where(x => x.ComplainReceiveId == complainReceiveInfo.ReceiveId && x.ProductId == complainReceiveInfo.ProductId).FirstOrDefault();
                    if (replacementClaimInfo != null)
                    {
                        complainReceiveInfo.Status = "Replacement Claim";

                        var replacementReceiveInfo = replacementReceiveInfoList.Where(x => x.ReplacementClaimId == replacementClaimInfo.ClaimId && x.NewProductId == complainReceiveInfo.ProductId).FirstOrDefault();
                        if (replacementReceiveInfo != null)
                        {
                            complainReceiveInfo.Status = "Replacement Receive";
                        }
                    }

                }
            }
        }
    }

    public class ComplainReceiveDetailInfoForAnalysis
    {
        private long _customerId;
        private long? _productDimensionId;
        private long _companyId;
        public ComplainReceiveDetailInfoForAnalysis(long customerId, long? productDimensionId, long companyId)
        {
            _customerId = customerId;
            _productDimensionId = productDimensionId;
            _companyId = companyId;
        }
        public string Location { get; set; }
        public string CustomerGroup { get; set; }
        public string Customer { get { return GenerateDifferentFullName.GenerateCustomerFull(_customerId, _companyId); } }
        public string CustomerPhoneNo { get; set; }        
        public Guid ReceiveId { get; set; }
        public string ReceiveNo { get; set; }
        public string InvoiceNo { get; set; }        
        public DateTime ReceiveDate { get; set; }
        public string ReceivedBy { get; set; }
        public string ReceivedByContactNo { get; set; }
        public string ApprovedBy { get; set; }        
        public string EntryBy { get; set; }
        public string ProductGroup { get; set; }
        public string Model { get; set; }
        public string ProductCode { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }        
        public string Serial { get; set; }
        public string Remarks { get; set; }        
        public List<Problem> ProblemNames { get; set; }        
        public string ProductDimension { get { return GenerateDifferentFullName.GenerateProductDimensionFull(_productDimensionId, _companyId); } }
        public string UnitType { get; set; }
        public string DeliveryStatus { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Status { get; set; }
        public string DeliveryNumber { get; set; }
        public string SettlementType { get; set; }
        public string DeliveredProductName { get; set; }
        public string DeliveredSerial { get; set; }
        public int DeliveryDaysTaken { get; set; }
    }
    public class Problem
    {
        public string Name { get; set; }
    }

    //public class CollectionModeInfo
    //{
    //    public Guid InvoiceId { get; set; }
    //    public string CollectionMode { get; set; }
    //    public decimal Amount { get; set; }
    //}
}
