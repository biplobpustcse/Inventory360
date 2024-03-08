using System;
using System.Collections.Generic;

namespace Inventory360Web.Models
{
    public class CommonTaskGoodsReceive : CommonHeader
    {
        public string ReceiveNo { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string OrderNo { get; set; }
        public string SuppliedBy { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime? ReferenceDate { get; set; }
        public string Remarks { get; set; }
        public List<CommonTaskGoodsReceiveDetail> ReceiveDetailLists { get; set; }
    }

    public class CommonTaskGoodsReceiveDetail
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductDimension { get; set; }
        public string UnitType { get; set; }
        public decimal Quantity { get; set; }
        public string Warehouse { get; set; }
        public bool SerialAvailable { get; set; }
        public List<CommonTaskProductSerial> SerialLists { get; set; }
    }
}