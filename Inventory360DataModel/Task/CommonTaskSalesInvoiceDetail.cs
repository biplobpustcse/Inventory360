using System;
using System.Collections.Generic;

namespace Inventory360DataModel.Task
{
    public class CommonTaskSalesInvoiceDetail
    {
        public Guid InvoiceDetailId { get; set; }
        public Guid InvoiceId { get; set; }
        public Guid ChallanId { get; set; }
        public long ProductId { get; set; }
        public long? ProductDimensionId { get; set; }
        public long UnitTypeId { get; set; }
        public long PrimaryUnitTypeId { get; set; }
        public long? SecondaryUnitTypeId { get; set; }
        public long? TertiaryUnitTypeId { get; set; }
        public decimal SecondaryConversionRatio { get; set; }
        public decimal TertiaryConversionRatio { get; set; }
        public long? WarehouseId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Price1 { get; set; }
        public decimal Price2 { get; set; }
        public decimal Discount { get; set; }
        public decimal Discount1 { get; set; }
        public decimal Discount2 { get; set; }
        public decimal Cost { get; set; }
        public decimal Cost1 { get; set; }
        public decimal Cost2 { get; set; }
        public List<CommonTaskProductSerial> SerialLists { get; set; }
    }
}