namespace Inventory360DataModel.Setup
{
    public class CommonSetupProduct
    {
        public long ProductId { get; set; }
        public string Code { get; set; }
        public long ProductGroupId { get; set; }
        public long? ProductSubGroupId { get; set; }
        public long ProductCategoryId { get; set; }
        public long BrandId { get; set; }
        public string Model { get; set; }
        public string Name { get; set; }
        public string ShortSpecification { get; set; }
        public string WebName { get; set; }
        public bool SerialAvailable { get; set; }
        public bool IsActive { get; set; }
        public long PrimaryUnitTypeId { get; set; }
        public long? SecondaryUnitTypeId { get; set; }
        public long? TertiaryUnitTypeId { get; set; }
        public decimal SecondaryConversionRatio { get; set; }
        public decimal TertiaryConversionRatio { get; set; }
        public decimal ReorderPoint { get; set; }
        public decimal MinStockQuantity { get; set; }
        public decimal ReturnAllowedDays { get; set; }
        public decimal CreditNoteAllowedDays { get; set; }
        public decimal DebitNoteAllowedDays { get; set; }
        public bool IsStockMaintain { get; set; }
        public bool IsSaleable { get; set; }
        public string ProductType { get; set; }
        public string StockType { get; set; }
        public long? AccountsId { get; set; }
        public string AccountsName { get; set; }
        public bool IsWarrantyAvailable { get; set; }
        public bool IsLifeTimeWarranty { get; set; }
        public decimal WarrantyDays { get; set; }
        public decimal AdditionalWarrantyDays { get; set; }
        public bool IsServiceWarranty { get; set; }
        public decimal ServiceWarrantyDays { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
    }
}