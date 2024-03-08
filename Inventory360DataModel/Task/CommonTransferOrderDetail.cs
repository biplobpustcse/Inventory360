using System;

namespace Inventory360DataModel.Task
{
    public class CommonTransferOrderDetail
    {
        public Guid TransferOrderDetailId { get; set; }
        public Guid TransferOrderId { get; set; }
        public Guid? RequisitionFinalizeId { get; set; }
        public long ProductId { get; set; }
        public long? ProductDimensionId { get; set; }
        public long UnitTypeId { get; set; }
        public decimal Quantity { get; set; }
    }
}