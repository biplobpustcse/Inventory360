using System;
using System.Collections.Generic;

namespace Inventory360Web.Models
{
    public class CommonReplacementReceiveAnalysisReport : CommonHeader
    {
        public string CurrencyType { get; set; }
        public string ReportName { get; set; }
        public string DateRange { get; set; }
        public List<ReplacementReceiveAnalysisInfo> ReplacementReceiveAnalysisLists { get; set; }
    }

    public class ReplacementReceiveAnalysisInfo
    {
        public string Location { get; set; }
        public string SupplierGroup { get; set; }
        public string Supplier { get; set; }
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
        public List<Problem> ProblemNames { get; set; }
        public string ProductDimension { get; set; }
        public string UnitType { get; set; }
        public string ClaimProductName { get; set; }
        public long ClaimProductId { get; set; }
        public string ClaimProductUnitType { get; set; }
        public string ClaimProductSerial { get; set; }
    }
}