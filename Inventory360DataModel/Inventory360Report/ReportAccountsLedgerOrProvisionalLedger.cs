using Inventory360DataModel.Temp;
using System;
using System.Collections.Generic;

namespace Inventory360DataModel.Inventory360Report
{
    public class ReportAccountsLedgerOrProvisionalLedger : CommonHeader
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public List<TempAccountsLedgerOrProvisionalLedger> LedgerLists { get; set; }
    }
}