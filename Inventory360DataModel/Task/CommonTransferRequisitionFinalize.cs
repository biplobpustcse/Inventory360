using System;
using System.Collections.Generic;

namespace Inventory360DataModel.Task
{
    public class CommonTransferRequisitionFinalize
    {
        public Guid RequisitionId { get; set; }
        public string RequisitionNo { get; set; }
        public DateTime RequisitionDate { get; set; }
        public long RequisitionBy { get; set; }
        public string Remarks { get; set; }
        public string StockType { set; get; }
        public int TransferToLocationId { set; get; }
        public long LocationId { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
        public List<CommonTransferRequisitionFinalizeDetail> FinalizeDetailLists { get; set; }
    }
}