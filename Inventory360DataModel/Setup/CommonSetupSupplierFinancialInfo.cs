using System;

namespace Inventory360DataModel.Setup
{
    public class CommonSetupSupplierFinancialInfo
    {
        public long SupplierId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string SelectedCurrency { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal OpeningBalance { get; set; }
        public DateTime? OpeningDate { get; set; }
        public long? AccountsId { get; set; }
        public string AccountsName { get; set; }
        public long CompanyId { get; set; }
    }
}