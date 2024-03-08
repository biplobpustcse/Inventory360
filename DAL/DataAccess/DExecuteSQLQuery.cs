using Inventory360Entity;
using DAL.Interface;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess
{
    public class DExecuteSQLQuery : IExecuteSQLQuery
    {
        private Inventory360Entities _db;

        public DExecuteSQLQuery()
        {
            _db = new Inventory360Entities();
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public string ExecuteQuery(string sqlQuery)
        {
            return _db.Database.SqlQuery<string>(sqlQuery)
                .FirstOrDefault()
                .Trim();
        }
    }
}