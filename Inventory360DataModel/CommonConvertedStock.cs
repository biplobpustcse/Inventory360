namespace Inventory360DataModel
{
    public class CommonConvertedStock
    {
        public decimal PrimaryStockQty { get; set; }
        public decimal PrimaryStockCost { get; set; }
        public decimal SecondaryStockQty { get; set; }
        public decimal TertiaryStockQty { get; set; }
        public int UnitLevel { get; set; }
    }
}