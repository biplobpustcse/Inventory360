using Inventory360Entity;
using DAL.Interface.Select.Setup;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Setup
{
    public class DSelectSetupUnitType : ISelectSetupUnitType
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectSetupUnitType(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Setup_UnitType> SelectUnitTypeAll()
        {
            return _db.Setup_UnitType
                .Where(x => x.CompanyId == _companyId);
        }
    }
}
