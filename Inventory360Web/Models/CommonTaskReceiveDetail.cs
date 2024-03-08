using System;
using System.Collections.Generic;

namespace Inventory360Web.Models
{
    public class CommonTaskReceiveDetail : CommonHeader
    {
        public string FinalizeNo { get; set; }
        public DateTime FinalizeDate { get; set; }
        public string SupplierCode { get; set; }
        public string ContactNo { get; set; }
        public string SupplierName { get; set; }
        public string SupplierAddress { get; set; }
        public decimal FinalizeAmount { get; set; }
        public string AmountInWord { get; set; }
        public string CurrencyType { get; set; }
        public List<CommonTaskReceiveFinalizeDetail> DetailLists { get; set; }
    }

    public class CommonTaskReceiveFinalizeDetail
    {
        public string ReceiveNo { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string OrderNo { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductDimension { get; set; }
        public string UnitType { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }
}