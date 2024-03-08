using Inventory360Entity;
using DAL.Interface.Select.Setup;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Setup
{
    public class DSelectSetupSupplier : ISelectSetupSupplier
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectSetupSupplier(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Setup_Supplier> SelectSupplierAll()
        {
            return _db.Setup_Supplier
                .Where(x => x.CompanyId == _companyId);
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Setup_Supplier> SelectSupplierWithoutCheckingCompany()
        {
            return _db.Setup_Supplier;
        }
    }
}