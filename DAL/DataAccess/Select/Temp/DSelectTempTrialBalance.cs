using Inventory360Entity;
using DAL.Interface.Select.Temp;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Temp
{
    public class DSelectTempTrialBalance : ISelectTempTrialBalance
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTempTrialBalance(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Temp_TrialBalance> SelectTempTrialBalanceAll()
        {
            return _db.Temp_TrialBalance
                .Where(x => x.CompanyId == _companyId);
        }
    }
}