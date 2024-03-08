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
    public class GridReportCustomerDeliveryAnalysisReport
    {
        public object GenerateCustomerDeliveryAnalysisReport(long locationId, string dateFrom, string dateTo, Guid complainReceiveId, long customerGroupId, long customerId, long groupId, long subGroupId, long categoryId, long brandId, string model, long productId, string currency, long companyId)
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

                ISelectTaskCustomerDeliveryDetail iSelectTaskCustomerDeliveryDetail = new DSelectTaskCustomerDeliveryDetail(companyId);
                var customerDeliveryInfo = iSelectTaskCustomerDeliveryDetail.SelectCustomerDeliveryDetailAll()
                    .WhereIf(locationId > 0, x => x.Task_CustomerDelivery.LocationId == locationId)
                    .WhereIf(fromDate != null, x => DbFunctions.TruncateTime(x.Task_CustomerDelivery.DeliveryDate) >= DbFunctions.TruncateTime(fromDate))
                    .WhereIf(toDate != null, x => DbFunctions.TruncateTime(x.Task_CustomerDelivery.DeliveryDate) <= DbFunctions.TruncateTime(toDate))
                    .WhereIf(customerGroupId > 0, x => x.Task_CustomerDelivery.Setup_Customer.CustomerGroupId == customerGroupId)
                    .WhereIf(customerId > 0, x => x.Task_CustomerDelivery.CustomerId == customerId)
                    .WhereIf(groupId > 0, x => x.Setup_Product.ProductGroupId == groupId)
                    .WhereIf(subGroupId > 0, x => x.Setup_Product.ProductSubGroupId == subGroupId)
                    .WhereIf(categoryId > 0, x => x.Setup_Product.ProductCategoryId == categoryId)
                    .WhereIf(brandId > 0, x => x.Setup_Product.BrandId == brandId)
                    .WhereIf(!string.IsNullOrEmpty(model), x => x.Setup_Product.Model.ToLower().Contains(model.ToLower()))
                    .WhereIf(productId > 0, x => x.NewProductId == productId)
                    .Select(s => new
                    {
                        Location = s.Task_CustomerDelivery.Setup_Location.Name,
                        CustomerGroup = s.Task_CustomerDelivery.Setup_Customer.Setup_CustomerGroup.Name,
                        s.Task_CustomerDelivery.CustomerId,
                        s.Task_CustomerDelivery.Setup_Customer.SalesPersonId,
                        s.Task_CustomerDelivery.Setup_Customer.PhoneNo,
                        s.DeliveryId,
                        s.Task_CustomerDelivery.DeliveryNo,
                        DeliveryDate = (DateTime)DbFunctions.TruncateTime(s.Task_CustomerDelivery.DeliveryDate),
                        DeliveryBy = s.Task_CustomerDelivery.Setup_Employee.Name,
                        DeliveryByContactNo = s.Task_CustomerDelivery.Setup_Employee.ContactNo,
                        DeliveryType = "",
                        ApprovedBy = s.Task_CustomerDelivery.Security_User.UserName,
                        EntryBy = s.Task_CustomerDelivery.Security_User1.UserName,
                        DeliveryProductGroup = s.Setup_Product1.Setup_ProductGroup.Name,
                        DeliveryProductModel = s.Setup_Product1.Model,
                        DeliveryProductCode = s.Setup_Product1.Code,
                        DeliveryProductName = s.Setup_Product1.Name,
                        DeliveryProductSerial = s.NewSerial,
                        PreviousProductName = s.Setup_Product.Name,
                        PreviousProductSerial = s.PreviousSerial,
                        DeliveryProductDimensionId = s.NewProductDimensionId,
                        DeliveryProductUnitType = s.Setup_UnitType1.Name,
                        TotalSpareAmount = s.TotalSpareAmount,
                        TotalServiceAmount = s.TotalServiceAmount,
                        AdjustedAmount = s.AdjustedAmount,
                        AdjustmentType = s.AdjustmentType,
                        ComplainReceiveDetail_Problem = s.Task_CustomerDeliveryDetail_Problem.Select(x => new Problem { Name = x.Setup_Problem.Name }).ToList()
                    })
                    .ToList()
                    .Select(s => new CustomerDeliveryDetailInfoForAnalysis(s.CustomerId, s.DeliveryProductDimensionId, companyId)
                    {
                        Location = s.Location,
                        CustomerGroup = s.CustomerGroup,
                        DeliveryId = s.DeliveryId,
                        DeliveryNo = s.DeliveryNo,
                        DeliveryType = s.DeliveryType,
                        DeliveryDate = s.DeliveryDate,
                        CustomerPhoneNo = s.PhoneNo,
                        DeliveryBy = s.DeliveryBy,
                        DeliveryByContactNo = s.DeliveryByContactNo,
                        ApprovedBy = s.ApprovedBy,
                        EntryBy = s.EntryBy,
                        DeliveryProductGroup = s.DeliveryProductGroup,
                        DeliveryProductModel = s.DeliveryProductModel,
                        DeliveryProductCode = s.DeliveryProductCode,
                        DeliveryProductName = s.DeliveryProductName,
                        DeliveryProductSerial = s.DeliveryProductSerial,
                        PreviousProductName = s.PreviousProductName,
                        PreviousProductSerial = s.PreviousProductSerial,
                        DeliveryProductUnitType = s.DeliveryProductUnitType,
                        ProblemNames = s.ComplainReceiveDetail_Problem,
                        TotalSpareAmount = s.TotalSpareAmount,
                        TotalServiceAmount = s.TotalServiceAmount,
                        AdjustedAmount = s.AdjustedAmount,
                        AdjustmentType = s.AdjustmentType
                    }).ToList();
                return new
                {
                    companyInfo.CompanyName,
                    companyInfo.CompanyAddress,
                    companyInfo.Phone,
                    companyInfo.Fax,
                    CustomerDeliveryAnalysisLists = customerDeliveryInfo
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    public class CustomerDeliveryDetailInfoForAnalysis
    {
        private long _customerId;
        private long? _productDimensionId;
        private long _companyId;
        public CustomerDeliveryDetailInfoForAnalysis(long customerId, long? productDimensionId, long companyId)
        {
            _customerId = customerId;
            _productDimensionId = productDimensionId;
            _companyId = companyId;
        }
        public string Location { get; set; }
        public string CustomerGroup { get; set; }
        public string Customer { get { return GenerateDifferentFullName.GenerateCustomerFull(_customerId, _companyId); } }
        public string CustomerPhoneNo { get; set; }        
        public Guid DeliveryId { get; set; }
        public string DeliveryNo { get; set; }   
        public string DeliveryType { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DeliveryBy { get; set; }
        public string DeliveryByContactNo { get; set; }
        public string ApprovedBy { get; set; }        
        public string EntryBy { get; set; }
        public string DeliveryProductGroup { get; set; }
        public string DeliveryProductModel { get; set; }
        public string DeliveryProductCode { get; set; }
        public string DeliveryProductName { get; set; }
        public string DeliveryProductSerial { get; set; }
        public string PreviousProductName { get; set; }
        public string PreviousProductSerial { get; set; }
        public List<Problem> ProblemNames { get; set; }        
        public string DeliveryProductDimension { get { return GenerateDifferentFullName.GenerateProductDimensionFull(_productDimensionId, _companyId); } }
        public string DeliveryProductUnitType { get; set; }
        public Decimal TotalSpareAmount { get; set; }
        public Decimal TotalServiceAmount { get; set; }
        public Decimal AdjustedAmount { get; set; }
        public string AdjustmentType { get; set; }
        public decimal TotalAmount
        {
            get
            {
                try
                {
                    // calculate by value
                    return (TotalSpareAmount + TotalServiceAmount + (AdjustmentType == "A" ? AdjustedAmount : AdjustmentType == "D" ? (AdjustedAmount *(-1)) : 0));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
