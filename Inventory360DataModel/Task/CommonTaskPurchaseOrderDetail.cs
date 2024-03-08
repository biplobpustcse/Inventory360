using System;

namespace Inventory360DataModel.Task
{
    public class CommonTaskPurchaseOrderDetail
    {
        public Guid OrderDetailId { get; set; }
        public Guid OrderId { get; set; }
        public Guid? RequisitionId { get; set; }
        public long ProductId { get; set; }
        public long? ProductDimensionId { get; set; }
        public long UnitTypeId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }
}