namespace Inventory360DataModel.Setup
{
    public class CommonSetupRelatedProduct
    {
        public long RelatedProductId { get; set; }
        public long ProductId { get; set; }
        public long RelatedOrSpareProductId { get; set; }
        public string Type { get; set; }
        public long EntryBy { get; set; }
    }
}