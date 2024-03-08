namespace Inventory360DataModel.Setup
{
    public class CommonSetupSupplier
    {
        public long SupplierId { get; set; }
        public long SupplierGroupId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string URL { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPersonMobile { get; set; }
        public long? ProfessionId { get; set; }
        public string Designation { get; set; }
        public long? BankId { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountNumber { get; set; }
        public bool IsActive { get; set; }
        public long CompanyId { get; set; }
        public long LocationId { get; set; }
        public long EntryBy { get; set; }
        public string SelectedCurrency { get; set; }
    }
}