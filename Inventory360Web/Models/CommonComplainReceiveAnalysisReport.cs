using System;
using System.Collections.Generic;

namespace Inventory360Web.Models
{
    public class CommonComplainReceiveAnalysisReport : CommonHeader
    {
        public string CurrencyType { get; set; }
        public string ReportName { get; set; }
        public string DateRange { get; set; }
        public List<ComplainReceivAnalysisInfo> ComplainReceiveAnalysisLists { get; set; }
    }

    public class ComplainReceivAnalysisInfo
    {
        public string Location { get; set; }
        public string CustomerGroup { get; set; }
        public string Customer { get; set; }
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
        public string ProductName { get; set; }
        public string Serial { get; set; }
        public string Remarks { get; set; }
        public List<Problem> ProblemNames { get; set; }
        public string ProductDimension { get; set; }
        public decimal Quantity { get; set; }
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
}