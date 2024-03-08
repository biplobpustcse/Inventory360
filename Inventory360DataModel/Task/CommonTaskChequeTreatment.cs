using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventory360DataModel.Task
{
    public class CommonTaskChequeTreatment
    {
        CommonList commonList = new CommonList();
        public Guid TreatmentId { get; set; }
        public Guid ChequeInfoId { get; set; }
        public string PreviousStatus { get; set; }
        public string Status { get; set; }
        public string StatusName { get { return commonList.SelectChequeStatus().Where(x => x.Value == Status).Select(s => s.Item).FirstOrDefault(); } }
        public DateTime StatusDate { get; set; }
        public long TreatmentBankId { get; set; }
        public long PreviousTreatmentBankId { get; set; }
        public Guid? VoucherId { get; set; }       
        public DateTime EntryDate { get; set; }       
        public long EntryBy { get; set; }       
        public List<CommonTaskChequeTreatment> CommonTaskChequeTreatmentLists { get; set; }
    }
}