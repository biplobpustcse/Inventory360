using Inventory360Entity;
using DAL.Interface.Select.Setup;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Setup
{
    public class DSelectSetupCustomer : ISelectSetupCustomer
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectSetupCustomer(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Setup_Customer> SelectCustomerAll()
        {
            return _db.Setup_Customer
                .Where(x => x.CompanyId == _companyId);
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Setup_Customer> SelectCustomerWithoutCheckingCompany()
        {
            return _db.Setup_Customer;
        }
    }
}