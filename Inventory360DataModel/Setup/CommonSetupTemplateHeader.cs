namespace Inventory360DataModel.Setup
{
    public class CommonSetupTemplateHeader
    {
        public long TemplateHeaderId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
    }
}