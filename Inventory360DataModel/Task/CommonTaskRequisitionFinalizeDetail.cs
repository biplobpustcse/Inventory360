using System;

namespace Inventory360DataModel.Task
{
    public class CommonTaskRequisitionFinalizeDetail
    {
        public Guid RequisitionDetailId { get; set; }
        public Guid RequisitionId { get; set; }
        public Guid? ItemRequisitionId { get; set; }
        public long ProductId { get; set; }
        public long? ProductDimensionId { get; set; }
        public long UnitTypeId { get; set; }
        public decimal Quantity { get; set; }
    }
}