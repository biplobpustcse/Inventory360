using System;
using System.Collections.Generic;

namespace Inventory360DataModel.Temp
{
    public class TempAccountsLedgerOrProvisionalLedger
    {
        public DateTime Date { get; set; }
        public string VoucherNo { get; set; }
        public string VoucherDescription { get; set; }
        public decimal DrAmount { get; set; }
        public decimal CrAmount { get; set; }
        public List<TempAccountsLedgerDetail> TempDetail { get; set; }
    }

    public class TempAccountsLedgerDetail
    {
        public string AccountsName { get; set; }
        public decimal DrAmount { get; set; }
        public decimal CrAmount { get; set; }
    }
}