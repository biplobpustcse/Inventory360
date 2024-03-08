using Inventory360Entity;
using DAL.Interface.Select.Setup;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Setup
{
    public class DSelectSetupBrand : ISelectSetupBrand
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectSetupBrand(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Setup_Brand> SelectBrandAll()
        {
            return _db.Setup_Brand
                .Where(x => x.CompanyId == _companyId);
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Setup_Brand> SelectBrandWithoutCheckingCompany()
        {
            return _db.Setup_Brand;
        }
    }
}
