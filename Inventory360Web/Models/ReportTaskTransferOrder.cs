using System;
using System.Collections.Generic;

namespace Inventory360Web.Models
{
    public class ReportTaskTransferOrder : CommonHeader
    {
        public string TransferOrderNo { get; set; }
        public DateTime TransferOrderDate { get; set; }
        public string FromLocation { set; get; }
        public string TransferFromStockType { get; set; }
        public string ToLocation { set; get; }
        public string TransferToStockType { get; set; }
        public string TransferOrderBy { get; set; }
        public List<ReportTaskTransferOrderDetail> DetailLists { get; set; }
    }

    public class ReportTaskTransferOrderDetail
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductDimension { get; set; }
        public string UnitType { get; set; }
        public decimal Quantity { get; set; }
    }
}