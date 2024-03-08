using Inventory360Entity;
using DAL.Interface.Select.Setup;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Setup
{
    public class DSelectSetupTermsAndConditions : ISelectSetupTermsAndConditions
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectSetupTermsAndConditions(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Setup_TermsAndConditions> SelectTermsAndConditionsAll()
        {
            return _db.Setup_TermsAndConditions
                .Where(x => x.CompanyId == _companyId);
        }
    }
}