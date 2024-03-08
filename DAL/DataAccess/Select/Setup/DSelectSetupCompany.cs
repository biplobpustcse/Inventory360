using Inventory360Entity;
using DAL.Interface.Select.Setup;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Setup
{
    //http://thedatafarm.com/data-access/installing-ef-power-tools-into-vs2015/
    public class DSelectSetupCompany : ISelectSetupCompany
    {
        private Inventory360Entities _db;

        public DSelectSetupCompany()
        {
            _db = new Inventory360Entities();
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Setup_Company> SelectCompanyAll()
        {
            Inventory360Entities _entity = new Inventory360Entities();
            return _entity.Setup_Company;
        }
    }
}