using System;

namespace Inventory360DataModel.Task
{
    public class CommonTaskSalesOrderDetail
    {
        public Guid SalesOrderDetailId { get; set; }
        public Guid SalesOrderId { get; set; }
        public long ProductId { get; set; }
        public long? ProductDimensionId { get; set; }
        public long UnitTypeId { get; set; }
        public long PrimaryUnitTypeId { get; set; }
        public long? SecondaryUnitTypeId { get; set; }
        public long? TertiaryUnitTypeId { get; set; }
        public decimal SecondaryConversionRatio { get; set; }
        public decimal TertiaryConversionRatio { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
    }

    public class CommonTaskSalesOrderDetailAtEdit
    {
        public long ProductId { get; set; }
        public string Name { get; set; }
        public long? ProductDimensionId { get; set; }
        public string Dimension { get; set; }
        public long UnitTypeId { get; set; }
        public string UnitTypeName { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get { return (Quantity * (Price - Discount)); } }
    }
}