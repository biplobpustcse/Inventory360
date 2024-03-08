using Inventory360Entity;
using DAL.Interface.Select.Configuration;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Configuration
{
    public class DSelectConfigurationOperationalEventDetail : ISelectConfigurationOperationalEventDetail
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectConfigurationOperationalEventDetail(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Configuration_OperationalEventDetail> SelectOperationalEventDetailAll()
        {
            return _db.Configuration_OperationalEventDetail;
        }
    }
}