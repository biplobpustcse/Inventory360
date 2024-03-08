using Inventory360Entity;
using DAL.Interface.Select.Others;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Others
{
    public class DSelectOthersReport : ISelectOthersReport
    {
        private Inventory360Entities _db;

        public DSelectOthersReport()
        {
            _db = new Inventory360Entities();
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Others_Report> SelectReportNameAll()
        {
            return _db.Others_Report;
        }
    }
}