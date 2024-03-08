using System;
using System.Collections.Generic;

namespace Inventory360DataModel.Task
{
    public class CommonTaskStockAdjustmentDetail
    {
        public Guid AdjustmentDetailId { get; set; }
        public Guid AdjustmentId { get; set; }
        public long ProductId { get; set; }
        public long? ProductDimensionId { get; set; }
        public long UnitTypeId { get; set; }
        public long PrimaryUnitTypeId { get; set; }
        public long? SecondaryUnitTypeId { get; set; }
        public long? TertiaryUnitTypeId { get; set; }
        public decimal SecondaryConversionRatio { get; set; }
        public decimal TertiaryConversionRatio { get; set; }
        public decimal Quantity { get; set; }
        public string OperationType { get; set; }
        public long? WarehouseId { get; set; }
        public decimal Cost { get; set; }
        public List<CommonTaskProductSerial> SerialLists { get; set; }
    }
}