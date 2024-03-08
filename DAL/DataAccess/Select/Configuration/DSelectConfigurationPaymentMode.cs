using Inventory360Entity;
using DAL.Interface.Select.Configuration;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Configuration
{
    public class DSelectConfigurationPaymentMode : ISelectConfigurationPaymentMode
    {
        private Inventory360Entities _db;

        public DSelectConfigurationPaymentMode()
        {
            _db = new Inventory360Entities();
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Configuration_PaymentMode> SelectPaymentModeAll()
        {
            return _db.Configuration_PaymentMode;
        }
    }
}