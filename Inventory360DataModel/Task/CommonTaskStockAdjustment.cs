using System;
using System.Collections.Generic;

namespace Inventory360DataModel.Task
{
    public class CommonTaskStockAdjustment
    {
        public Guid AdjustmentId { get; set; }
        public string AdjustmentNo { get; set; }
        public DateTime AdjustmentDate { get; set; }
        public long? EmployeeId { get; set; }
        public string SelectedCurrency { get; set; }
        public decimal ExchangeRate { get; set; }
        public string Remarks { get; set; }
        public long LocationId { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
        public List<CommonTaskStockAdjustmentDetail> StockAdjustmentDetailList { get; set; }
    }
}