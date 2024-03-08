using System.Collections.Generic;

namespace Inventory360Web.Models
{
    public class CommonStockCurrentStock : CommonHeader
    {
        public bool WithSerial { get; set; }
        public List<StockInfo> StockInfoLists { get; set; }
    }

    public class StockInfo
    {
        public string Location { get; set; }
        public string WareHouse { get; set; }
        public string Group { get; set; }
        public string SubGroup { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Product { get; set; }
        public string Dimension { get; set; }
        public decimal Quantity { get; set; }
        public string UnitType { get; set; }
        public bool SerialAvailable { get; set; }
        public List<CommonTaskProductSerial> SerialLists { get; set; }
    }
}