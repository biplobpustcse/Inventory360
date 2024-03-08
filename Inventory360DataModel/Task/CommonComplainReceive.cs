using System;
using System.Collections.Generic;

namespace Inventory360DataModel.Task
{
    public class CommonComplainReceive
    {
        public Guid ReceiveId { get; set; }
        public string ReceiveNo { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string SelectedCurrency { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool AgainstPreviousSales { get; set; }
        public long CustomerId { get; set; }
        public long RequestedBy { get; set; }
        public decimal TotalChargeAmount { get; set; }
        public string Remarks { get; set; }
        public long LocationId { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
        public List<CommonComplainReceive_Charge> ComplainReceive_Charge { get; set; }
        public List<CommonComplainReceiveDetail> ComplainReceiveDetail { get; set; }
    }
}