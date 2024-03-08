namespace Inventory360DataModel.Setup
{
    public class CommonSetupPriceType
    {
        public long PriceTypeId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsDetailPrice { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
    }
}