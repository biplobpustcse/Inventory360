using System;

namespace Inventory360DataModel.Task
{
    public class CommonTaskCustomerDeliveryDetail_SpareProduct
    {
        public System.Guid DeliveryDetailSpareId { get; set; }
        public System.Guid DeliveryDetailId { get; set; }
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
        public string SpareType { get; set; }
        public string Remarks { get; set; }
    }
}