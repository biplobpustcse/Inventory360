using System;
using System.Collections.Generic;

namespace Inventory360Web.Models
{
    public class CommonStockAdjustment : CommonHeader
    {
        public string AdjustmentNo { get; set; }
        public DateTime AdjustmentDate { get; set; }
        public string RequestedBy { get; set; }
        public string Remarks { get; set; }
        public string CurrencyType { get; set; }
        public List<CommonStockAdjustmentDetail> AdjustmentDetailLists { get; set; }
    }

    public class CommonStockAdjustmentDetail
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductDimension { get; set; }
        public string UnitType { get; set; }
        public string IncreaseDecrease { get; set; }
        public decimal Quantity { get; set; }
        public decimal Cost { get; set; }
        public string WareHouse { get; set; }
        public bool SerialAvailable { get; set; }
        public List<CommonTaskProductSerial> SerialLists { get; set; }
    }
}