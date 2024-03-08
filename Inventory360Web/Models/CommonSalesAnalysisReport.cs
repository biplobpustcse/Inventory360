using System;
using System.Collections.Generic;

namespace Inventory360Web.Models
{
    public class CommonSalesAnalysisReport : CommonHeader
    {
        public string CurrencyType { get; set; }
        public string ReportName { get; set; }
        public string DateRange { get; set; }
        public List<SalesAnalysisInfo> SalesAnalysisLists { get; set; }
        public List<CollectionDetailInfo> CollectionDetails { get; set; }
    }

    public class SalesAnalysisInfo
    {
        public string Location { get; set; }
        public string CustomerGroup { get; set; }
        public string Customer { get; set; }
        public string SalesPerson { get; set; }
        public string SalesMode { get; set; }
        public Guid InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string ApprovedBy { get; set; }
        public string EntryBy { get; set; }
        public string ProductGroup { get; set; }
        public string Model { get; set; }
        public bool IsProductCodeVisible { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductDimension { get; set; }
        public decimal Quantity { get; set; }
        public string UnitType { get; set; }
        public decimal ProductWiseAmount { get; set; }
        public decimal ProductWiseDiscount { get; set; }
        public decimal ProductWiseCost { get; set; }
        public decimal InvDiscPerProduct { get; set; }
        public decimal InvCollectionPerProduct { get; set; }
    }

    public class CollectionDetailInfo
    {
        public Guid InvoiceId { get; set; }
        public string CollectionMode { get; set; }
        public decimal Amount { get; set; }
    }
}