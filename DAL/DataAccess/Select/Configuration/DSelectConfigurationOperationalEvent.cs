using Inventory360Entity;
using DAL.Interface.Select.Configuration;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Configuration
{
    public class DSelectConfigurationOperationalEvent : ISelectConfigurationOperationalEvent
    {
        private Inventory360Entities _db;

        public DSelectConfigurationOperationalEvent()
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