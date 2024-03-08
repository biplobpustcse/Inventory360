using System;
using System.Collections.Generic;

namespace Inventory360DataModel.Task
{
    public class CommonTaskConvertionDetail
    {
        public Guid ConvertionDetailId { get; set; }
        public Guid ConvertionId { get; set; }
        public string ProductFor { get; set; }
        public long ProductId { get; set; }
        public Nullable<long> ProductDimensionId { get; set; }
        public long UnitTypeId { get; set; }
        public Nullable<long> WareHouseId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Cost { get; set; }
        public decimal Cost1 { get; set; }
        public decimal Cost2 { get; set; }        
        public long PrimaryUnitTypeId { get; set; }
        public Nullable<long> SecondaryUnitTypeId { get; set; }
        public Nullable<long> TertiaryUnitTypeId { get; set; }
        public decimal SecondaryConversionRatio { get; set; }
        public decimal TertiaryConversionRatio { get; set; }
        public string ReferenceNo { get; set; }
        public Nullable<System.DateTime> ReferenceDate { get; set; }
        public Nullable<System.Guid> GoodsReceiveId { get; set; }
        public Nullable<System.Guid> ImportedStockInId { get; set; }
        public Nullable<long> SupplierId { get; set; }
        public List<CommonTaskConvertionDetailSerial> CommonTaskConvertionDetailSerial { get; set; }
    }
}