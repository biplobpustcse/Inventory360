namespace Inventory360DataModel
{
    public class CommonSetupBank
    {
        public long BankId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool IsOwnBank { get; set; }
        public string BankBranch { get; set; }
        public string BankACNo { get; set; }
        public long? AccountsId { get; set; }
        public string AccountsName { get; set; }
        public bool IsTransactionFound { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
    }
}
