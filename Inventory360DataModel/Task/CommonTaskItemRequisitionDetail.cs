﻿using System;

namespace Inventory360DataModel.Task
{
    public class CommonTaskItemRequisitionDetail
    {
        public Guid RequisitionDetailId { get; set; }
        public Guid RequisitionId { get; set; }
        public long ProductId { get; set; }
        public long? ProductDimensionId { get; set; }
        public long UnitTypeId { get; set; }
        public decimal Quantity { get; set; }
        public DateTime RequiredDate { get; set; }
        public string Reason { get; set; }
    }
}