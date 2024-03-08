using System.Linq;

namespace Inventory360DataModel.Temp
{
    public class TempStatusList
    {
        CommonList commonList = new CommonList();
        public string Status { get; set; }
        public string StatusName { get { return commonList.SelectChequeStatus().Where(x => x.Value == Status).Select(s => s.Item).FirstOrDefault(); } }
    }
}