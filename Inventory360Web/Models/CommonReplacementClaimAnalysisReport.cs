using System;
using System.Collections.Generic;

namespace Inventory360Web.Models
{
    public class CommonReplacementClaimAnalysisReport : CommonHeader
    {
        public string CurrencyType { get; set; }
        public string ReportName { get; set; }
        public string DateRange { get; set; }
        public List<ReplacementClaimAnalysisInfo> ReplacementClaimAnalysisLists { get; set; }
    }

    public class ReplacementClaimAnalysisInfo
    {
        public string Location { get; set; }
        public string SupplierGroup { get; set; }
        public string Supplier { get; set; }
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
        public string ProductDimension { get; set; }
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