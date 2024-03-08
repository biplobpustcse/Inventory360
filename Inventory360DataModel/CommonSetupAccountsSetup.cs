using System;
using System.Linq;

namespace Inventory360DataModel
{
    public class CommonSetupAccountsSetup
    {
        CommonList commonList = new CommonList();
        public long AccountsGroupId { get; set; }
        public string GroupName { get; set; }
        public long AccountsSubGroupId { get; set; }
        public string SubGroupName { get; set; }
        public long AccountsControlId { get; set; }
        public string ControlName { get; set; }
        public long AccountsSubsidiaryId { get; set; }
        public string SubsidiaryName { get; set; }
        public long AccountsId { get; set; }
        public string AccountsName { get; set; }
        public byte CategorizationId { get; set; }
        public string Categorization { get { return commonList.AccountsCategorization().Where(x => x.Id == CategorizationId).Select(s => s.Name).FirstOrDefault(); } }
        public DateTime OpeningDate { get; set; }
        public decimal OpeningBalance { get; set; }
        public string BalanceType { get; set; }
        public long LocationId { get; set; }
        public string LocationName { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
    }
}