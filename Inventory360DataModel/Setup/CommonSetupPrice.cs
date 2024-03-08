using System.Collections.Generic;

namespace Inventory360DataModel.Setup
{
    public class CommonSetupPrice
    {
        public long PriceId { get; set; }
        public string Currency { get; set; }
        public decimal ExchangeRate { get; set; }
        public long PriceTypeId { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public long? ProductDimensionId { get; set; }
        public long UnitTypeId { get; set; }
        public decimal Price { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
        public List<CommonRange> QuantityRange { get; set; }
    }
}