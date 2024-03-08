using Inventory360Entity;
using DAL.Interface.Select.Setup;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Setup
{
    public class DSelectSetupCountry : ISelectSetupCountry
    {
        private Inventory360Entities _db;

        public DSelectSetupCountry()
        {
            _db = new Inventory360Entities();
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Setup_Country> SelectCountryAll()
        {
            return _db.Setup_Country;
        }
    }
}