namespace Inventory360DataModel.Setup
{
    public class CommonSetupLocation
    {
        public long LocationId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsLoginLocation { get; set; }
        public long CompanyId { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public long? InChargeId { get; set; }
        public string InChargeName { get; set; }
        public bool IsWareHouse { get; set; }
        public long? MasterLocationId { get; set; }
        public bool IsPortLocation { get; set; }
        public long EntryBy { get; set; }
        public string DefaultCurrency { get; set; }
    }
}