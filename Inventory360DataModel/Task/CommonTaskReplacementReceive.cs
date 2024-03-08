using System;
using System.Collections.Generic;

namespace Inventory360DataModel.Task
{
    public class CommonTaskReplacementReceive
    {
        public Guid ReceiveId { get; set; }
        public string ReceiveNo { get; set; }
        public string ReceiveDate { get; set; }
        public string SelectedCurrency { get; set; }
        public decimal Currency1Rate { get; set; }
        public decimal Currency2Rate { get; set; }
        public long RequestedBy { get; set; }
        public string Remarks { get; set; }
        public decimal TotalChargeAmount { get; set; }
        public decimal TotalChargeAmount1 { get; set; }
        public decimal TotalChargeAmount2 { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalDiscount1 { get; set; }
        public decimal TotalDiscount2 { get; set; }
        public long LocationId { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
        public List<CommonTaskReplacementReceiveDetail> ReplacementReceiveDetail { get; set; }
        public List<CommonTaskReplacementReceive_Charge> ReplacementReceiveCharge { get; set; }
    }
}