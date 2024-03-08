using System;
using System.Collections.Generic;

namespace Inventory360DataModel.Task
{
    public class CommonTaskGoodsReceiveDetail
    {
        public Guid ReceiveDetailId { get; set; }
        public Guid ReceiveId { get; set; }
        public long ProductId { get; set; }
        public long? ProductDimensionId { get; set; }
        public long UnitTypeId { get; set; }
        public decimal Quantity { get; set; }
        public long? WarehouseId { get; set; }
        public List<CommonTaskProductSerial> SerialLists { get; set; }
    }
}