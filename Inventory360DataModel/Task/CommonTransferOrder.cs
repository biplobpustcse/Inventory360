using System;
using System.Collections.Generic;

namespace Inventory360DataModel.Task
{
    public class CommonTransferOrder
    {
        public Guid OrderId { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public long OrderBy { get; set; }
        public long TransferToId { get; set; }
        public string TransferToStockType { get; set; }
        public long TransferFromId { get; set; }
        public string TransferFromStockType { get; set; }
        public string Remarks { get; set; }
        public long LocationId { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
        public List<CommonTransferOrderDetail> TransferOrderDetailList { get; set; }
    }
}