namespace Inventory360DataModel.Setup
{
    public class CommonSetupProductAdditionalInfo
    {
        public long AdditionalInfoId { get; set; }
        public long ProductId { get; set; }
        public decimal Height { get; set; }
        public decimal Width { get; set; }
        public decimal Thickness { get; set; }
        public long? CapacityId { get; set; }
        public decimal Capacity { get; set; }
        public long? OriginCountryId { get; set; }
        public string HSCode { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
    }
}