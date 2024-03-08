using Inventory360Entity;
using DAL.Interface.Select.Setup;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Setup
{
    public class DSelectSetupStyle : ISelectSetupStyle
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectSetupStyle(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Setup_Style> SelectStyleAll()
        {
            return _db.Setup_Style
                .Where(x => x.CompanyId == _companyId);
        }
    }
}