using System;
using System.Collections.Generic;

namespace Inventory360Web.Models
{
    public class CommonLedger : CommonHeader
    {
        public string LedgerFor { get; set; }
        public string PartyCode { get; set; }
        public string PartyPhone { get; set; }
        public string PartyName { get; set; }
        public string PartyAddress { get; set; }
        public string CurrencyType { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public List<CommonLedgerDetail> DetailLists { get; set; }
    }

    public class CommonLedgerDetail
    {
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Particular { get; set; }
        public decimal DrAmount { get; set; }
        public decimal CrAmount { get; set; }
    }
}