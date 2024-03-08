using System;

namespace Inventory360DataModel.Setup
{
    public class CommonSetupCustomerFinancialInfo
    {
        public long CustomerId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string BaseCurrency { get; set; }
        public string SelectedCurrency { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal OpeningBalance { get; set; }
        public DateTime? OpeningDate { get; set; }
        public decimal ChequeDishonourBalance { get; set; }
        public DateTime? ChequeDishonourOpeningDate { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal LedgerLimit { get; set; }
        public decimal CreditAllowedDays { get; set; }
        public bool IsLocked { get; set; }
        public bool IsRMALocked { get; set; }
        public long? AccountsId { get; set; }
        public string AccountsName { get; set; }
        public long CompanyId { get; set; }
    }
}