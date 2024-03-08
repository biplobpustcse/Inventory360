using System;
using System.Collections.Generic;

namespace Inventory360Web.Models
{
    public class CommonTaskRequisitionFinalize : CommonHeader
    {
        public string RequisitionNo { get; set; }
        public DateTime RequisitionDate { get; set; }
        public string RequestedBy { get; set; }
        public string Remarks { get; set; }
        public List<CommonTaskRequisitionFinalizeDetail> RequisitionDetailLists { get; set; }
    }

    public class CommonTaskRequisitionFinalizeDetail
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductDimension { get; set; }
        public string UnitType { get; set; }
        public decimal Quantity { get; set; }
        public string Identity { get; set; }
        public string ItemRequisitionNo { get; set; }
    }
}