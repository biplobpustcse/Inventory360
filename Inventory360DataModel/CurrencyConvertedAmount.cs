namespace Inventory360DataModel
{
    public class CurrencyConvertedAmount
    {
        public decimal BaseAmount { get; set; }
        public decimal Currency1Rate { get; set; }
        public decimal Currency1Amount { get; set; }
        public decimal Currency2Rate { get; set; }
        public decimal Currency2Amount { get; set; }
    }
}