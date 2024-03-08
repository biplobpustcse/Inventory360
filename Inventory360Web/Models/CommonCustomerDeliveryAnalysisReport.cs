using System;
using System.Collections.Generic;

namespace Inventory360Web.Models
{
    public class CommonCustomerDeliveryAnalysisReport : CommonHeader
    {
        public string CurrencyType { get; set; }
        public string ReportName { get; set; }
        public string DateRange { get; set; }
        public List<CustomerDeliveryAnalysisInfo> CustomerDeliveryAnalysisLists { get; set; }
    }

    public class CustomerDeliveryAnalysisInfo
    {
        public string Location { get; set; }
        public string CustomerGroup { get; set; }
        public string Customer { get; set; }
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
        public string DeliveryProductDimension { get; set; }
        public string DeliveryProductUnitType { get; set; }
        public Decimal TotalSpareAmount { get; set; }
        public Decimal TotalServiceAmount { get; set; }
        public Decimal AdjustedAmount { get; set; }
        public string AdjustmentType { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
