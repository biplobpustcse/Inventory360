using Inventory360Entity;
using DAL.Interface.Select.Setup;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Setup
{
    public class DSelectSetupTransportType : ISelectSetupTransportType
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectSetupTransportType(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Setup_TransportType> SelectTransportTypeAll()
        {
            return _db.Setup_TransportType
                .Where(x => x.CompanyId == _companyId);
        }
    }
}