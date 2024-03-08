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
    public class GridReportReplacementReceiveAnalysis
    {
        public object GenerateReplacementReceiveAnalysisReport(long locationId, string dateFrom, string dateTo, Guid replacementReceiveId, long supplierGroupId, long supplierId, long groupId, long subGroupId, long categoryId, long brandId, string model, long productId, string currency, long companyId)
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

                ISelectTaskReplacementReceiveDetail iSelectTaskReplacementReceiveDetail = new DSelectTaskReplacementReceiveDetail(companyId);
                var replacementReceiveInfo = iSelectTaskReplacementReceiveDetail.SelectReplacementReceiveDetailAll()
                    .Where(x => x.Task_ReplacementReceive.Approved == "A")
                    .WhereIf(locationId > 0, x => x.Task_ReplacementReceive.LocationId == locationId)
                    .WhereIf(locationId > 0, x => x.Task_ReplacementReceive.LocationId == locationId)
                    .WhereIf(replacementReceiveId != Guid.Empty, x => x.ReceiveId == replacementReceiveId)
                    .WhereIf(fromDate != null, x => DbFunctions.TruncateTime(x.Task_ReplacementReceive.ReceiveDate) >= DbFunctions.TruncateTime(fromDate))
                    .WhereIf(toDate != null, x => DbFunctions.TruncateTime(x.Task_ReplacementReceive.ReceiveDate) <= DbFunctions.TruncateTime(toDate))
                    .WhereIf(supplierGroupId > 0, x => x.Task_ReplacementClaim.Setup_Supplier.SupplierGroupId == supplierGroupId)
                    .WhereIf(supplierId > 0, x => x.Task_ReplacementClaim.SupplierId == supplierId)
                    .WhereIf(groupId > 0, x => x.Setup_Product.ProductGroupId == groupId)
                    .WhereIf(subGroupId > 0, x => x.Setup_Product.ProductSubGroupId == subGroupId)
                    .WhereIf(categoryId > 0, x => x.Setup_Product.ProductCategoryId == categoryId)
                    .WhereIf(brandId > 0, x => x.Setup_Product.BrandId == brandId)
                    .WhereIf(!string.IsNullOrEmpty(model), x => x.Setup_Product.Model.ToLower().Contains(model.ToLower()))
                    .WhereIf(productId > 0, x => x.NewProductId == productId)
                    .Select(s => new
                    {
                        Location = s.Task_ReplacementReceive.Setup_Location.Name,
                        SupplierGroup = s.Task_ReplacementClaim.Setup_Supplier.Setup_SupplierGroup.Name,                        
                        s.Task_ReplacementClaim.SupplierId,
                        SupplierPhoneNo = s.Task_ReplacementClaim.Setup_Supplier.Phone,
                        s.ReceiveId,
                        s.Task_ReplacementReceive.ReceiveNo,
                        s.Task_ReplacementReceive.ReceiveDate,
                        s.Task_ReplacementReceive.Remarks,
                        s.Task_ReplacementReceive.TotalChargeAmount,
                        s.Task_ReplacementReceive.TotalDiscount,
                        ReceivedBy = s.Task_ReplacementReceive.Setup_Employee.Name,
                        ReceivedByContactNo = s.Task_ReplacementReceive.Setup_Employee.ContactNo,
                        ApprovedBy = s.Task_ReplacementReceive.Security_User.UserName,
                        EntryBy = s.Task_ReplacementReceive.Security_User1.UserName,
                        AdjustedAmount = s.AdjustedAmount,
                        ReceivedModel = s.Setup_Product1.Model,
                        ReceivedProductGroup = s.Setup_Product1.Setup_ProductGroup.Name,
                        ReceivedProductBrand = s.Setup_Product1.Setup_Brand.Name,
                        ReceivedProductCategory = s.Setup_Product1.Setup_ProductCategory.Name,
                        ReceivedProductCode = s.Setup_Product1.Code,
                        ReceivedProductName = s.Setup_Product1.Name,                       
                        ReceivedProductId = s.NewProductId,
                        s.NewSerial,
                        s.NewProductDimensionId,
                        UnitType = s.Setup_UnitType1.Name,
                        ClaimProductName = s.Setup_Product.Name,
                        ClaimProductId = s.PreviousProductId,
                        ClaimProductUnitType = s.Setup_UnitType.Name,
                        ClaimProductSerial = s.NewSerial,
                        ReplacementClaimDetail_Problem = s.Task_ReplacementClaim.Task_ReplacementClaimDetail.Where(x=>x.ProductId == s.PreviousProductId && x.Serial == s.PreviousSerial).FirstOrDefault().Task_ReplacementClaimDetail_Problem.Select(x => new Problem { Name = x.Setup_Problem.Name }).ToList()
                    })
                    .ToList()
                    .Select(s => new ReplacementReceiveDetailInfoForAnalysis(s.SupplierId, s.NewProductDimensionId, companyId)
                    {
                        Location = s.Location,
                        SupplierGroup = s.SupplierGroup,                        
                        ReceiveId = s.ReceiveId,
                        ReceiveNo = s.ReceiveNo,
                        ReceiveDate = s.ReceiveDate,
                        Remarks = s.Remarks,
                        AdjustedAmount = s.AdjustedAmount,
                        TotalChargeAmount = s.TotalChargeAmount,
                        TotalDiscount = s.TotalDiscount,
                        SupplierPhoneNo = s.SupplierPhoneNo,
                        ReceivedBy = s.ReceivedBy,
                        ReceivedByContactNo = s.ReceivedByContactNo,
                        ApprovedBy = s.ApprovedBy,
                        EntryBy = s.EntryBy,
                        ReceivedModel = s.ReceivedModel,
                        ReceivedProductGroup = s.ReceivedProductGroup,
                        ReceivedProductBrand = s.ReceivedProductBrand,
                        ReceivedProductCategory = s.ReceivedProductCategory,
                        ReceivedProductCode = s.ReceivedProductCode,
                        ReceivedProductName = s.ReceivedProductName,
                        ReceivedProductId = s.ReceivedProductId,
                        Serial = s.NewSerial,
                        UnitType = s.UnitType,
                        ClaimProductName = s.ClaimProductName,
                        ClaimProductId = s.ClaimProductId,
                        ClaimProductUnitType = s.ClaimProductUnitType,
                        ClaimProductSerial = s.ClaimProductSerial,
                        ProblemNames = s.ReplacementClaimDetail_Problem
                    }).ToList();

                return new
                {
                    companyInfo.CompanyName,
                    companyInfo.CompanyAddress,
                    companyInfo.Phone,
                    companyInfo.Fax,
                    ReplacementReceiveAnalysisLists = replacementReceiveInfo
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class ReplacementReceiveDetailInfoForAnalysis
    {
        private long _supplierId;
        private long? _productDimensionId;
        private long _companyId;
        public ReplacementReceiveDetailInfoForAnalysis(long supplierId, long? productDimensionId, long companyId)
        {
            _supplierId = supplierId;
            _productDimensionId = productDimensionId;
            _companyId = companyId;
        }
        public string Location { get; set; }
        public string SupplierGroup { get; set; }
        public string Supplier { get { return GenerateDifferentFullName.GenerateSupplierFull(_supplierId, _companyId); } }
        public string SupplierPhoneNo { get; set; }
        public Guid ReceiveId { get; set; }
        public string ReceiveNo { get; set; }
        public DateTime ReceiveDate { get; set; }        
        public decimal AdjustedAmount { get; set; }
        public decimal TotalChargeAmount { get; set; }        
        public decimal TotalDiscount { get; set; }
        public string ReceivedBy { get; set; }
        public string ReceivedByContactNo { get; set; }
        public string ApprovedBy { get; set; }
        public string EntryBy { get; set; }
        public string ReceivedProductGroup { get; set; }
        public string ReceivedProductBrand { get; set; }
        public string ReceivedProductCategory { get; set; }
        public string ReceivedModel { get; set; }
        public string ReceivedProductCode { get; set; }
        public long ReceivedProductId { get; set; }
        public string ReceivedProductName { get; set; }
        public string Serial { get; set; }
        public string Remarks { get; set; }
        public long ClaimProductId { get; set; }
        public string ClaimProductName { get; set; }
        public string ClaimProductSerial { get; set; }
        public string ClaimProductUnitType { get; set; }
        public List<Problem> ProblemNames { get; set; }
        public string ProductDimension { get { return GenerateDifferentFullName.GenerateProductDimensionFull(_productDimensionId, _companyId); } }
        public string UnitType { get; set; }       
    }
}
