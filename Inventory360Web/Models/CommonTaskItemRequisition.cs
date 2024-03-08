using System;
using System.Collections.Generic;

namespace Inventory360Web.Models
{
    public class CommonTaskItemRequisition : CommonHeader
    {
        public string RequisitionNo { get; set; }
        public DateTime RequisitionDate { get; set; }
        public string RequisitionBy { get; set; }
        public string CollectedBy { get; set; }
        public string Remarks { get; set; }
        public List<CommonTaskItemRequisitionDetail> RequisitionDetailLists { get; set; }
    }

    public class CommonTaskItemRequisitionDetail
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductDimension { get; set; }
        public string UnitType { get; set; }
        public decimal Quantity { get; set; }
        public DateTime RequiredDate { get; set; }
        public string Reason { get; set; }
    }
}