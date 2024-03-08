namespace Inventory360DataModel.Setup
{
    public class CommonSetupProductDimension
    {
        public long ProductDimensionId { get; set; }
        public long ProductId { get; set; }
        public long? MeasurementId { get; set; }
        public long? SizeId { get; set; }
        public long? StyleId { get; set; }
        public long? ColorId { get; set; }
        public long EntryBy { get; set; }
        public long CompanyId { get; set; }
    }
}