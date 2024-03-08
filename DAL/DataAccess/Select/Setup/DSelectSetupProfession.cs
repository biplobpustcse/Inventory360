using Inventory360Entity;
using DAL.Interface.Select.Setup;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Setup
{
    public class DSelectSetupProfession : ISelectSetupProfession
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectSetupProfession(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Setup_Profession> SelectProfessionAll()
        {
            return _db.Setup_Profession
                .Where(x => x.CompanyId == _companyId);
        }
    }
}