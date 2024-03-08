using System;

namespace Inventory360DataModel
{
    public class CommonTaskPostedVoucher
    {
        public Guid VoucherDetailId { get; set; }
        public DateTime VoucherDate { get; set; }
        public string VoucherType { get; set; }
        public long AccountsId { get; set; }
        public long ProjectId { get; set; }
        public decimal Amount { get; set; }
        public decimal Rate1 { get; set; }
        public decimal Amount1 { get; set; }
        public decimal Rate2 { get; set; }
        public decimal Amount2 { get; set; }
        public long LocationId { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
    }
}