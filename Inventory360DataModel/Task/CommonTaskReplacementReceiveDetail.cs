using System;
using System.Collections.Generic;

namespace Inventory360DataModel.Task
{
    public class CommonTaskReplacementReceiveDetail
    {
        public Guid ReceiveDetailId { get; set; }
        public Guid ReceiveId { get; set; }
        public Guid ReplacementClaimId { get; set; }
        public long PreviousProductId { get; set; }
        public Nullable<long> PreviousProductDimensionId { get; set; }
        public long PreviousUnitTypeId { get; set; }
        public string PreviousSerial { get; set; }
        public long NewProductId { get; set; }
        public Nullable<long> NewProductDimensionId { get; set; }
        public long NewUnitTypeId { get; set; }
        public string NewSerial { get; set; }
        public decimal Cost { get; set; }
        public decimal Cost1 { get; set; }
        public decimal Cost2 { get; set; }
        public long ReplacementType { get; set; }
        public string AdjustmentType { get; set; }
        public decimal AdjustedAmount { get; set; }
        public decimal AdjustedAmount1 { get; set; }
        public decimal AdjustedAmount2 { get; set; }
    }
}