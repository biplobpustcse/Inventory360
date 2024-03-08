using System;

namespace Inventory360DataModel.Task
{
    public class CommonComplainReceiveDetail_SpareProduct
    {
        public Guid ReceiveDetailSpareId { get; set; }
        public Guid ReceiveDetailId { get; set; }
        public long ProductId { get; set; }
        public Nullable<long> ProductDimensionId { get; set; }
        public long UnitTypeId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Price1 { get; set; }
        public decimal Price2 { get; set; }
        public decimal Discount { get; set; }
        public decimal Discount1 { get; set; }
        public decimal Discount2 { get; set; }
    }
}