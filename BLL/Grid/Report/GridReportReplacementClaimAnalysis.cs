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
    public class GridReportReplacementClaimAnalysis
    {
        public object GenerateReplacementClaimAnalysisReport(long locationId, string dateFrom, string dateTo, Guid complainReceiveId, long supplierGroupId, long supplierId, long groupId, long subGroupId, long categoryId, long brandId, string model, long productId, string currency, long companyId)
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

                ISelectTaskReplacementClaimDetail iSelectTaskReplacementClaimDetail = new DSelectTaskReplacementClaimDetail(companyId);
                var replacementClaimInfo = iSelectTaskReplacementClaimDetail.SelectReplacementClaimDetailAll()
                    .WhereIf(locationId > 0, x => x.Task_ReplacementClaim.LocationId == locationId)
                    .WhereIf(locationId > 0, x => x.Task_ReplacementClaim.Approved == "A")
                    .WhereIf(fromDate != null, x => DbFunctions.TruncateTime(x.Task_ReplacementClaim.ClaimDate) >= DbFunctions.TruncateTime(fromDate))
                    .WhereIf(toDate != null, x => DbFunctions.TruncateTime(x.Task_ReplacementClaim.ClaimDate) <= DbFunctions.TruncateTime(toDate))
                    .WhereIf(supplierGroupId > 0, x => x.Task_ReplacementClaim.Setup_Supplier.SupplierGroupId == supplierGroupId)
                    .WhereIf(supplierId > 0, x => x.Task_ReplacementClaim.SupplierId == supplierId)
                    .WhereIf(groupId > 0, x => x.Setup_Product.ProductGroupId == groupId)
                    .WhereIf(subGroupId > 0, x => x.Setup_Product.ProductSubGroupId == subGroupId)
                    .WhereIf(categoryId > 0, x => x.Setup_Product.ProductCategoryId == categoryId)
                    .WhereIf(brandId > 0, x => x.Setup_Product.BrandId == brandId)
                    .WhereIf(!string.IsNullOrEmpty(model), x => x.Setup_Product.Model.ToLower().Contains(model.ToLower()))
                    .WhereIf(productId > 0, x => x.ProductId == productId)
                    .Select(s => new
                    {
                        Location = s.Task_ReplacementClaim.Setup_Location.Name,
                        SupplierGroup = s.Task_ReplacementClaim.Setup_Supplier.Setup_SupplierGroup.Name,
                        s.Task_ReplacementClaim.SupplierId,
                        SupplierPhoneNo = s.Task_ReplacementClaim.Setup_Supplier.Phone,
                        s.ClaimId,
                        s.Task_ReplacementClaim.ClaimNo,
                        s.Task_ReplacementClaim.Remarks,
                        ClaimDate = (DateTime)DbFunctions.TruncateTime(s.Task_ReplacementClaim.ClaimDate),
                        ReceivedBy = s.Task_ReplacementClaim.Setup_Employee.Name,
                        ReceivedByContactNo = s.Task_ReplacementClaim.Setup_Employee.ContactNo,
                        ApprovedBy = s.Task_ReplacementClaim.Security_User.UserName,
                        EntryBy = s.Task_ReplacementClaim.Security_User1.UserName,
                        ProductGroup = s.Setup_Product.Setup_ProductGroup.Name,
                        s.Setup_Product.Model,
                        ProductCode = s.Setup_Product.Code,
                        ProductName = s.Setup_Product.Name,
                        ProductId = s.ProductId,
                        s.Serial,
                        s.ProductDimensionId,
                        UnitType = s.Setup_UnitType.Name,
                        ComplainReceiveDetail_Problem = s.Task_ReplacementClaimDetail_Problem.Select(x => new Problem { Name = x.Setup_Problem.Name }).ToList()
                    })
                    .ToList()
                    .Select(s => new ReplacementClaimDetailInfoForAnalysis(s.SupplierId, s.ProductDimensionId, companyId)
                    {
                        Location = s.Location,
                        SupplierGroup = s.SupplierGroup,
                        ClaimId = s.ClaimId,
                        ClaimNo = s.ClaimNo,
                        Remarks = s.Remarks,
                        ClaimDate = s.ClaimDate,
                        SupplierPhoneNo = s.SupplierPhoneNo,
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
                        UnitType = s.UnitType,
                        ProblemNames = s.ComplainReceiveDetail_Problem
                    }).ToList();
                //ReceiveStatus
                ReplacemetStatus(replacementClaimInfo, companyId);

                return new
                {
                    companyInfo.CompanyName,
                    companyInfo.CompanyAddress,
                    companyInfo.Phone,
                    companyInfo.Fax,
                    ReplacementClaimAnalysisLists = replacementClaimInfo
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ReplacemetStatus(List<ReplacementClaimDetailInfoForAnalysis> replacementClaimInfoList, long companyId)
        {
            var claimIdIds = replacementClaimInfoList.Select(s => s.ClaimId).Distinct().ToList();
            //ReplacementReceive
            ISelectTaskReplacementReceiveDetail iSelectTaskReplacementReceiveDetail = new DSelectTaskReplacementReceiveDetail(companyId);
            var ReplacementReceiveInfoList = iSelectTaskReplacementReceiveDetail.SelectReplacementReceiveDetailAll()
                        .Where(x => claimIdIds.Contains((Guid)x.ReplacementClaimId))
                        .Select(
                            y => new
                            {
                                y.ReplacementClaimId,
                                y.Task_ReplacementReceive.ReceiveDate,
                                y.Task_ReplacementReceive.ReceiveId,
                                y.Task_ReplacementReceive.ReceiveNo,
                                DeliveredProductName = y.Setup_Product1.Name,
                                y.NewProductId,
                                y.NewSerial

                            }
                        ).ToList();
            //CustomerDelivery
            var receiveIdIds = ReplacementReceiveInfoList.Select(s => s.ReceiveId).Distinct().ToList();
            ISelectTaskCustomerDeliveryDetail iSelectTaskCustomerDeliveryDetail = new DSelectTaskCustomerDeliveryDetail(companyId);
            var DeliveryInfoList = iSelectTaskCustomerDeliveryDetail.SelectCustomerDeliveryDetailAll()
                        .Where(x => receiveIdIds.Contains((Guid)x.ComplainReceiveId))
                        .Select(
                            y => new
                            {
                                y.ComplainReceiveId,
                                y.Task_CustomerDelivery.DeliveryDate,
                                y.Task_CustomerDelivery.DeliveryNo,
                                DeliveredProductName = y.Setup_Product1.Name,
                                y.NewProductId,
                                y.NewSerial
                            }
                        ).ToList();

            foreach (ReplacementClaimDetailInfoForAnalysis replacementClaimInfo in replacementClaimInfoList)
            {
                var ReplacementReceiveInfo = ReplacementReceiveInfoList.Where(x => x.ReplacementClaimId == replacementClaimInfo.ClaimId && x.NewProductId == replacementClaimInfo.ProductId).FirstOrDefault();
                if (ReplacementReceiveInfo != null)
                {
                    replacementClaimInfo.Status = "Received";
                    replacementClaimInfo.ReplacementReceiveStatus = "Y";
                    replacementClaimInfo.ReplacementReceiveNumber = ReplacementReceiveInfo.ReceiveNo;
                    replacementClaimInfo.ReplacementReceiveDate = ReplacementReceiveInfo.ReceiveDate;
                    replacementClaimInfo.ReplacementReceiveProductName = ReplacementReceiveInfo.DeliveredProductName;
                    replacementClaimInfo.ReplacementReceiveSerial = ReplacementReceiveInfo.NewSerial;
                    replacementClaimInfo.ReplacementReceiveDaysTaken = (int)(ReplacementReceiveInfo.ReceiveDate - replacementClaimInfo.ClaimDate).TotalDays;
                    replacementClaimInfo.SettlementType = "";

                    var DeliveryInfo = DeliveryInfoList.Where(x => x.ComplainReceiveId == ReplacementReceiveInfo.ReceiveId && x.NewProductId == ReplacementReceiveInfo.NewProductId).FirstOrDefault();
                    if (DeliveryInfo != null)
                    {
                        replacementClaimInfo.DeliveryStatus = "Y";
                    }
                    else
                    {
                        replacementClaimInfo.DeliveryStatus = "N";
                    }
                }
                else
                {
                    replacementClaimInfo.ReplacementReceiveStatus = "N";
                    replacementClaimInfo.ReplacementReceiveDaysTaken = (int)(DateTime.Now - replacementClaimInfo.ClaimDate).TotalDays;
                }
            }
        }
    }

    public class ReplacementClaimDetailInfoForAnalysis
    {
        private long _supplierId;
        private long? _productDimensionId;
        private long _companyId;
        public ReplacementClaimDetailInfoForAnalysis(long supplierId, long? productDimensionId, long companyId)
        {
            _supplierId = supplierId;
            _productDimensionId = productDimensionId;
            _companyId = companyId;
        }
        public string Location { get; set; }
        public string SupplierGroup { get; set; }
        public string Supplier { get { return GenerateDifferentFullName.GenerateSupplierFull(_supplierId, _companyId); } }
        public string SupplierPhoneNo { get; set; }
        public Guid ClaimId { get; set; }
        public string ClaimNo { get; set; }
        public DateTime ClaimDate { get; set; }
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
        public string ReplacementReceiveStatus { get; set; }
        public DateTime ReplacementReceiveDate { get; set; }
        public string Status { get; set; }
        public string ReplacementReceiveNumber { get; set; }
        public string SettlementType { get; set; }
        public string ReplacementReceiveProductName { get; set; }
        public string ReplacementReceiveSerial { get; set; }
        public int ReplacementReceiveDaysTaken { get; set; }
    }
}
