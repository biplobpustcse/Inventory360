using System;
using System.Collections.Generic;

namespace Inventory360DataModel.Task
{
    public class CommonReplacementClaim
    {
        public Guid ClaimId { get; set; }
        public string ClaimNo { get; set; }
        public string ClaimDate { get; set; }
        public string SelectedCurrency { get; set; }
        public decimal Currency1Rate { get; set; }
        public decimal Currency2Rate { get; set; }
        public long RequestedBy { get; set; }
        public long SupplierId { get; set; }
        public string Remarks { get; set; }
        public bool IsSettledByReplacementReceive { get; set; }
        public long LocationId { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
        public List<CommonReplacementClaimDetail> replacementClaimDetail { get; set; }
    }
}