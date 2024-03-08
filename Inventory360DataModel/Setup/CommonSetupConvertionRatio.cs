using System;
using System.Collections.Generic;

namespace Inventory360DataModel.Setup
{
    public class CommonSetupConvertionRatio
    {       
        public System.Guid ConvertionRatioId { get; set; }
        public string RatioNo { get; set; }
        public string RatioTitle { get; set; }
        public System.DateTime RatioDate { get; set; }
        public string Description { get; set; }
        public string Approved { get; set; }
        public Nullable<long> ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public string CancelReason { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
        public System.DateTime EntryDate { get; set; }
        public List<CommonSetupConvertionRatioDetail> CommonSetupConvertionRatioDetail { get; set; }
    }
}