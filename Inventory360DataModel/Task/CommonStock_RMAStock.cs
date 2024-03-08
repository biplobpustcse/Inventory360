using System;
using System.Collections.Generic;

namespace Inventory360DataModel.Task
{
    public class CommonStock_RMAStock
    {
        public System.Guid RMAStockId { get; set; }
        public long ProductId { get; set; }
        public Nullable<long> ProductDimensionId { get; set; }
        public long UnitTypeId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Cost { get; set; }
        public decimal Cost1 { get; set; }
        public decimal Cost2 { get; set; }
        public string ReferenceNo { get; set; }
        public System.DateTime ReferenceDate { get; set; }
        public long LocationId { get; set; }
        public Nullable<long> WareHouseId { get; set; }
        public long CompanyId { get; set; }
        public System.DateTime EntryDate { get; set; }
        //public List<CommonStock_RMAStockSerial> stock_RMAStockSerial { get; set; }
    }
}