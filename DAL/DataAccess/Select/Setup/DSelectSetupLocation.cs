using Inventory360Entity;
using DAL.Interface.Select.Setup;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Setup
{
    public class DSelectSetupLocation : ISelectSetupLocation
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectSetupLocation(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Setup_Location> SelectLocationAll()
        {
            Inventory360Entities _entity = new Inventory360Entities();
            return _entity.Setup_Location
                .Where(x => x.CompanyId == _companyId);
        }
    }
}