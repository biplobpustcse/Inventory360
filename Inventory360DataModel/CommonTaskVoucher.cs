using System;
using System.Collections.Generic;

namespace Inventory360DataModel
{
    public class CommonTaskVoucher : CommonHeader
    {
        public Guid VoucherId { get; set; }
        public string VoucherNo { get; set; }
        public string VoucherType { get; set; }
        public string OperationType { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Approved { get; set; }
        public bool IsPosted { get; set; }
        public string Status { get { return GetStatus(Approved); } }
        public string CancelReason { get; set; }
        public long LocationId { get; set; }
        public long EntryBy { get; set; }
        public string EntryByName { get; set; }
        public string ApprovedBy { get; set; }
        public string Currency { get; set; }
        public decimal ExchangeRate { get; set; }

        public List<CommonTaskVoucherDetail> VoucherDetailLists { get; set; }
    }
}