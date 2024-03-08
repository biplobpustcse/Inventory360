using Inventory360Entity;
using DAL.Interface.Select.Configuration;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Configuration
{
    public class DSelectConfigurationOperationType : ISelectConfigurationOperationType
    {
        private Inventory360Entities _db;

        public DSelectConfigurationOperationType()
        {
            _db = new Inventory360Entities();
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Configuration_OperationType> SelectOperationTypeAll()
        {
            return _db.Configuration_OperationType;
        }
    }
}