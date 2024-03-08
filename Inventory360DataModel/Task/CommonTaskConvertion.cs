using System;
using System.Collections.Generic;

namespace Inventory360DataModel.Task
{
    public class CommonTaskConvertion
    {       
        public Guid ConvertionId { get; set; }
        public string ConvertionNo { get; set; }
        public System.DateTime ConvertionDate { get; set; }
        public string ConvertionType { get; set; }
        public string ConvertionFor { get; set; }
        public Nullable<Guid> ConvertionRatioId { get; set; }
        public string Remarks { get; set; }
        public string Approved { get; set; }
        public Nullable<long> ApprovedBy { get; set; }
        public Nullable<DateTime> ApprovedDate { get; set; }
        public string CancelReason { get; set; }
        public long LocationId { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
        public System.DateTime EntryDate { get; set; }
        public List<CommonTaskConvertionDetail> CommonTaskConvertionDetail { get; set; }        
    }
}