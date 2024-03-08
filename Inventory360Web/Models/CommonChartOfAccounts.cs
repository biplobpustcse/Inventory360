using System.Collections.Generic;

namespace Inventory360Web.Models
{
    public class CommonChartOfAccounts
    {
        public List<AccountsHead> HeadLists { get; set; }
    }

    public class AccountsHead
    {
        public string Group { get; set; }
        public string SubGroup { get; set; }
        public string Control { get; set; }
        public string Subsidiary { get; set; }
        public string Head { get; set; }
        public string BalanceType { get; set; }
    }
}