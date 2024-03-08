using System;
using System.Collections.Generic;

namespace Inventory360Web.Models
{
    public class CommonTaskVoucher
    {
        public Guid VoucherId { get; set; }
        public string VoucherNo { get; set; }
        public string VoucherType { get; set; }
        public string CurrencyType { get; set; }
        public string CurrencyCultureInfo { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string CancelReason { get; set; }
        public long LocationId { get; set; }
        public long CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyContact { get; set; }
        public string ApprovedBy { get; set; }
        public string EntryByName { get; set; }
        public string AmountInWord { get; set; }

        public List<CommonTaskVoucherDetail> VoucherDetailLists { get; set; }
    }

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