using System;
using System.Collections.Generic;

namespace Inventory360DataModel.Task
{
    public class CommonTaskGoodsReceive
    {
        public Guid ReceiveId { get; set; }
        public string ReceiveNo { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime? ReferenceDate { get; set; }
        public long SupplierId { get; set; }
        public Guid OrderId { get; set; }
        public string Remarks { get; set; }
        public string SelectedCurrency { get; set; }
        public decimal ExchangeRate { get; set; }
        public long LocationId { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
        public List<CommonTaskGoodsReceiveDetail> ReceiveDetailLists { get; set; }
    }
}