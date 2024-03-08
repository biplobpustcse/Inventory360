using Inventory360Entity;
using DAL.Interface.Select.Setup;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Setup
{
    public class DSelectSetupDesignation : ISelectSetupDesignation
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectSetupDesignation(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Setup_Designation> SelectDesignationAll()
        {
            return _db.Setup_Designation
                .Where(x => x.CompanyId == _companyId);
        }
    }
}
