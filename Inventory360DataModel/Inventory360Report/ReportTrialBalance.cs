using Inventory360DataModel.Temp;
using System.Collections.Generic;

namespace Inventory360DataModel.Inventory360Report
{
    public class ReportTrialBalance : CommonHeader
    {
        public List<TempTrialBalance> TrialBalanceLists { get; set; }
    }
}