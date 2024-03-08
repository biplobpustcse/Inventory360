using Inventory360Entity;
using DAL.Interface.Select.Setup;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Setup
{
    public class DSelectSetupMeasurement : ISelectSetupMeasurement
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectSetupMeasurement(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Setup_Measurement> SelectMeasurementAll()
        {
            return _db.Setup_Measurement
                .Where(x => x.CompanyId == _companyId);
        }
    }
}