using System;
using System.Collections.Generic;

namespace Inventory360DataModel.Task
{
    public class CommonTaskItemRequisition
    {
        public Guid RequisitionId { get; set; }
        public string RequisitionNo { get; set; }
        public DateTime RequisitionDate { get; set; }
        public long RequestedBy { get; set; }
        public string Remarks { get; set; }
        public long LocationId { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
        public List<CommonTaskItemRequisitionDetail> RequisitionDetailLists { get; set; }
    }
}