using System;
using System.Linq;

namespace Inventory360DataModel.Temp
{
   public class TempChequeTreatement
    {
        CommonList commonList = new CommonList();
        public Guid ChequeInfoId { get; set; }
        public string Status { get; set; }
        public string StatusName { get { return commonList.SelectChequeStatus().Where(x => x.Value == Status).Select(s => s.Item).FirstOrDefault(); } }
        public DateTime StatusDate { get; set; }
        public string BankName { get; set; }
    }
}