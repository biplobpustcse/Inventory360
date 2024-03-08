using System;
using System.Collections.Generic;

namespace Inventory360DataModel.Task
{
    public class CommonReplacementClaimDetail
    {
        public Guid ClaimDetailId { get; set; }
        public Guid ClaimId { get; set; }
        public Nullable<Guid> ComplainReceiveId { get; set; }
        public long ProductId { get; set; }
        public Nullable<long> ProductDimensionId { get; set; }
        public long UnitTypeId { get; set; }
        public string Serial { get; set; }
        public string AdditionalSerial { get; set; }
        public string ReceivedSerialNo { get; set; }
        public string ReceivedAdditionalSerial { get; set; }
        public string StockInBy { get; set; }
        public string StockInRefNo { get; set; }
        public Nullable<System.DateTime> StockInRefDate { get; set; }
        public string LCOrReferenceNo { get; set; }
        public Nullable<DateTime> LCOrReferenceDate { get; set; }
        public List<CommonReplacementClaimDetail_Problem> replacementClaimDetail_Problem { get; set; }
    }
}