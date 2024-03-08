using System;
using System.Collections.Generic;
using ZXing.OneD.RSS;

namespace Inventory360Web.Models
{
    public class ReportTaskItemRequisitionFinalization : CommonHeader
    {
        public string RequisitionId { get; set; }
        public string RequisitionNo { get; set; }
        public DateTime RequisitionDate { get; set; }
        public string StockType { get; set; }
        public string FromLocation { set; get; }
        public string ToLocation { set; get; }
        public string RequestedBy { get; set; }
        public string Currency { get; set; }
        public List<ReportTaskItemRequisitionFinalizationDetail> DetailLists { get; set; }
    }

    public class ReportTaskItemRequisitionFinalizationDetail
    {
        public long ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductDimensionId { set; get; }
        public string ProductDimension { get; set; }
        public long UnitTypeId { get; set; }
        public string UnitType { get; set; }
        public decimal Quantity { get; set; }
        public string FinalizedQuantity { set; get; }
    }
}