namespace Inventory360DataModel
{
    public class CommonSetupCashBankIdentification
    {
        public long IdentificationId { get; set; }
        public string IdentificationType { get; set; }
        public long? AccountsControlId { get; set; }
        public string ControlName { get; set; }
        public long? AccountsSubsidiaryId { get; set; }
        public string SubsidiaryName { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
    }
}