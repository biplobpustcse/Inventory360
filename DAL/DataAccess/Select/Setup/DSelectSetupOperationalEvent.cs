using Inventory360Entity;
using DAL.Interface.Select.Setup;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Setup
{
    public class DSelectSetupOperationalEvent : ISelectSetupOperationalEvent
    {
        private Inventory360Entities _db;

        public DSelectSetupOperationalEvent()
        {
            _db = new Inventory360Entities();
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Configuration_OperationalEvent> SelectOperationalEventAll()
        {
            return _db.Configuration_OperationalEvent;
        }
    }
}