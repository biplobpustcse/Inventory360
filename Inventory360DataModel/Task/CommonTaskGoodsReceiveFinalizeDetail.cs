using System;

namespace Inventory360DataModel.Task
{
    public class CommonTaskGoodsReceiveFinalizeDetail
    {
        public Guid FinalizeDetailId { get; set; }
        public Guid FinalizeId { get; set; }
        public Guid ReceiveId { get; set; }
        public long ProductId { get; set; }
        public long? ProductDimensionId { get; set; }
        public long UnitTypeId { get; set; }
        public decimal Quantity { get; set; }
    }
}