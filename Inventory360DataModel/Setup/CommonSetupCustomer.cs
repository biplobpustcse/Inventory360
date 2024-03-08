namespace Inventory360DataModel.Setup
{
    public class CommonSetupCustomer
    {
        public long CustomerId { get; set; }
        public long CustomerGroupId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string PhoneNo1 { get; set; }
        public string PhoneNo2 { get; set; }
        public long SalesPersonId { get; set; }
        public string SalesPersonName { get; set; }
        public bool IsCombined { get; set; }
        public bool IsActive { get; set; }
        public string Type { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPersonMobile { get; set; }
        public long? ProfessionId { get; set; }
        public string Designation { get; set; }
        public string ReferenceName { get; set; }
        public string ReferenceContactNo { get; set; }
        public long? SupplierId { get; set; }
        public long CompanyId { get; set; }
        public long LocationId { get; set; }
        public long EntryBy { get; set; }
        public string SelectedCurrency { get; set; }
        public string TransactionType { get; set; }
        public bool IsWalkIn { get; set; }
    }
}