using System;

namespace Inventory360DataModel
{
    public class CommonTaskVoucherDetail
    {
        public Guid VoucherDetailId { get; set; }
        public Guid VoucherId { get; set; }
        public long AccountsId { get; set; }
        public string AccountsName { get; set; }
        public string BalanceType { get; set; }
        public long ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Particulars { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
    }
}
