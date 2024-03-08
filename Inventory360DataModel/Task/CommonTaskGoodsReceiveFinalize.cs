using System;
using System.Collections.Generic;

namespace Inventory360DataModel.Task
{
    public class CommonTaskGoodsReceiveFinalize
    {
        public Guid FinalizeId { get; set; }
        public string FinalizeNo { get; set; }
        public DateTime FinalizeDate { get; set; }
        public long SupplierId { get; set; }
        public string SelectedCurrency { get; set; }
        public long LocationId { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
        public List<CommonTaskGoodsReceiveFinalizeDetail> FianlizeDetailLists { get; set; }
    }
}