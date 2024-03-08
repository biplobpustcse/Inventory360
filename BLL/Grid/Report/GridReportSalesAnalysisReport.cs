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
    public class GridReportSalesAnalysisReport
    {
        public object GenerateSalesAnalysisReport(long locationId, string dateFrom, string dateTo, long salesPersonId, string salesMode, long customerGroupId, long customerId, long groupId, long subGroupId, long categoryId, long brandId, string model, long productId, string currency, long companyId)
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

                ISelectTaskSalesInvoiceDetail iSelectTaskSalesInvoiceDetail = new DSelectTaskSalesInvoiceDetail(companyId);
                var salesInfo = iSelectTaskSalesInvoiceDetail.SelectSalesInvoiceDetailAll()
                    .Where(x => x.Task_SalesInvoice.Approved.Equals("A"))
                    .WhereIf(locationId > 0, x => x.Task_SalesInvoice.LocationId == locationId)
                    .WhereIf(fromDate != null, x => DbFunctions.TruncateTime(x.Task_SalesInvoice.InvoiceDate) >= DbFunctions.TruncateTime(fromDate))
                    .WhereIf(toDate != null, x => DbFunctions.TruncateTime(x.Task_SalesInvoice.InvoiceDate) <= DbFunctions.TruncateTime(toDate))
                    .WhereIf(salesPersonId > 0, x => x.Task_SalesInvoice.Setup_Customer.SalesPersonId == salesPersonId)
                    .WhereIf(salesMode.Equals("Cash"), x => x.Task_SalesInvoice.IsSalesModeCash)
                    .WhereIf(salesMode.Equals("Credit"), x => !x.Task_SalesInvoice.IsSalesModeCash)
                    .WhereIf(customerGroupId > 0, x => x.Task_SalesInvoice.Setup_Customer.CustomerGroupId == customerGroupId)
                    .WhereIf(customerId > 0, x => x.Task_SalesInvoice.CustomerId == customerId)
                    .WhereIf(groupId > 0, x => x.Setup_Product.ProductGroupId == groupId)
                    .WhereIf(subGroupId > 0, x => x.Setup_Product.ProductSubGroupId == subGroupId)
                    .WhereIf(categoryId > 0, x => x.Setup_Product.ProductCategoryId == categoryId)
                    .WhereIf(brandId > 0, x => x.Setup_Product.BrandId == brandId)
                    .WhereIf(!string.IsNullOrEmpty(model), x => x.Setup_Product.Model.ToLower().Contains(model.ToLower()))
                    .WhereIf(productId > 0, x => x.ProductId == productId)
                    .Select(s => new
                    {
                        Location = s.Task_SalesInvoice.Setup_Location.Name,
                        CustomerGroup = s.Task_SalesInvoice.Setup_Customer.Setup_CustomerGroup.Name,
                        s.Task_SalesInvoice.CustomerId,
                        s.Task_SalesInvoice.Setup_Customer.SalesPersonId,
                        SalesMode = s.Task_SalesInvoice.IsSalesModeCash ? "Cash" : "Credit",
                        s.InvoiceId,
                        s.Task_SalesInvoice.InvoiceNo,
                        InvoiceDate = (DateTime)DbFunctions.TruncateTime(s.Task_SalesInvoice.InvoiceDate),
                        ApprovedBy = s.Task_SalesInvoice.Security_User.UserName,
                        EntryBy = s.Task_SalesInvoice.Security_User1.UserName,
                        ProductGroup = s.Setup_Product.Setup_ProductGroup.Name,
                        s.Setup_Product.Model,
                        ProductCode = s.Setup_Product.Code,
                        ProductName = s.Setup_Product.Name,
                        s.ProductDimensionId,
                        s.Quantity,
                        UnitType = s.Setup_UnitType.Name,
                        ProductWiseAmount = currencyInfo.BaseCurrency == currency ? (s.Price * s.Quantity) : (currencyInfo.Currency1 == currency ? (s.Price1 * s.Quantity) : (s.Price2 * s.Quantity)),
                        ProductWiseDiscount = currencyInfo.BaseCurrency == currency ? (s.Discount * s.Quantity) : (currencyInfo.Currency1 == currency ? (s.Discount1 * s.Quantity) : (s.Discount2 * s.Quantity)),
                        InvoiceFullAmount = currencyInfo.BaseCurrency == currency ? s.Task_SalesInvoice.InvoiceAmount : (currencyInfo.Currency1 == currency ? s.Task_SalesInvoice.Invoice1Amount : s.Task_SalesInvoice.Invoice2Amount),
                        InvoiceFullDiscount = currencyInfo.BaseCurrency == currency ? s.Task_SalesInvoice.InvoiceDiscount : (currencyInfo.Currency1 == currency ? s.Task_SalesInvoice.Invoice1Discount : s.Task_SalesInvoice.Invoice2Discount),
                        InvoiceFullCollection = currencyInfo.BaseCurrency == currency ? s.Task_SalesInvoice.CollectedAmount : (currencyInfo.Currency1 == currency ? s.Task_SalesInvoice.Collected1Amount : s.Task_SalesInvoice.Collected2Amount)
                    })
                    .ToList()
                    .Select(s => new InvoiceDetailInfoForAnalysis(s.CustomerId, s.SalesPersonId, s.ProductDimensionId, companyId)
                    {
                        Location = s.Location,
                        CustomerGroup = s.CustomerGroup,
                        SalesMode = s.SalesMode,
                        InvoiceId = s.InvoiceId,
                        InvoiceNo = s.InvoiceNo,
                        InvoiceDate = s.InvoiceDate,
                        ApprovedBy = s.ApprovedBy,
                        EntryBy = s.EntryBy,
                        ProductGroup = s.ProductGroup,
                        Model = s.Model,
                        ProductCode = s.ProductCode,
                        ProductName = s.ProductName,
                        Quantity = s.Quantity,
                        UnitType = s.UnitType,
                        ProductWiseAmount = s.ProductWiseAmount,
                        ProductWiseDiscount = s.ProductWiseDiscount,
                        InvoiceFullAmount = s.InvoiceFullAmount,
                        InvoiceFullDiscount = s.InvoiceFullDiscount,
                        InvoiceFullCollection = s.InvoiceFullCollection
                    })
                    .ToList();

                var invoiceIds = salesInfo.Select(s => s.InvoiceId).Distinct().ToList();
                ISelectTaskCollectionMapping iSelectTaskCollectionMapping = new DSelectTaskCollectionMapping(companyId);
                var collectionDetails = iSelectTaskCollectionMapping.SelectCollectionMappingAll()
                    .Where(x => x.Task_Collection.Approved.Equals("A")
                        && invoiceIds.Contains((Guid)x.InvoiceId))
                    .WhereIf(fromDate != null, x => DbFunctions.TruncateTime(x.Task_Collection.CollectionDate) >= DbFunctions.TruncateTime(fromDate))
                    .WhereIf(toDate != null, x => DbFunctions.TruncateTime(x.Task_Collection.CollectionDate) <= DbFunctions.TruncateTime(toDate))
                    .Select(s => new
                    {
                        s.InvoiceId,
                        CollectionInfo = s.Task_Collection.Task_CollectionDetail.GroupBy(g => new { g.PaymentModeId }, (key, g) => new
                        {
                            CollectionMode = g.Select(ss => ss.Configuration_PaymentMode.Name).FirstOrDefault(),
                            Amount = g.Sum(ss => currencyInfo.BaseCurrency == currency ? ss.Amount : (currencyInfo.Currency1 == currency ? ss.Amount1 : ss.Amount2))
                        }).ToList()
                    })
                    .ToList();

                List<CollectionModeInfo> finalCollectionDetailLists = new List<CollectionModeInfo>();
                foreach (var item in collectionDetails)
                {
                    foreach (var item1 in item.CollectionInfo)
                    {
                        finalCollectionDetailLists.Add(new CollectionModeInfo { InvoiceId = (Guid)item.InvoiceId, CollectionMode = item1.CollectionMode, Amount = item1.Amount });
                    }
                }

                return new
                {
                    companyInfo.CompanyName,
                    companyInfo.CompanyAddress,
                    companyInfo.Phone,
                    companyInfo.Fax,
                    SalesAnalysisLists = salesInfo,
                    CollectionDetails = finalCollectionDetailLists
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class InvoiceDetailInfoForAnalysis
    {
        private long _customerId;
        private long _salesPersonId;
        private long? _productDimensionId;
        private long _companyId;
        public InvoiceDetailInfoForAnalysis(long customerId, long salesPersonId, long? productDimensionId, long companyId)
        {
            _customerId = customerId;
            _salesPersonId = salesPersonId;
            _productDimensionId = productDimensionId;
            _companyId = companyId;
        }
        public string Location { get; set; }
        public string CustomerGroup { get; set; }
        public string Customer { get { return GenerateDifferentFullName.GenerateCustomerFull(_customerId, _companyId); } }
        public string SalesPerson { get { return GenerateDifferentFullName.GenerateEmployeeFull(_salesPersonId, _companyId); } }
        public string SalesMode { get; set; }
        public Guid InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string ApprovedBy { get; set; }
        public string EntryBy { get; set; }
        public string ProductGroup { get; set; }
        public string Model { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductDimension { get { return GenerateDifferentFullName.GenerateProductDimensionFull(_productDimensionId, _companyId); } }
        public decimal Quantity { get; set; }
        public string UnitType { get; set; }
        public decimal ProductWiseAmount { get; set; }
        public decimal ProductWiseDiscount { get; set; }
        public decimal InvoiceFullAmount { get; set; }
        public decimal InvoiceFullDiscount { get; set; }
        public decimal InvDiscPerProduct
        {
            get
            {
                try
                {
                    // calculate by value ratio
                    return (InvoiceFullDiscount / (InvoiceFullAmount == 0 ? 1 : InvoiceFullAmount)) * (ProductWiseAmount - ProductWiseDiscount);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public decimal InvoiceFullCollection { get; set; }
        public decimal InvCollectionPerProduct
        {
            get
            {
                try
                {
                    // calculate by value ratio
                    return (InvoiceFullCollection / (InvoiceFullAmount == 0 ? 1 : InvoiceFullAmount)) * (ProductWiseAmount - ProductWiseDiscount);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }

    public class CollectionModeInfo
    {
        public Guid InvoiceId { get; set; }
        public string CollectionMode { get; set; }
        public decimal Amount { get; set; }
    }
}
