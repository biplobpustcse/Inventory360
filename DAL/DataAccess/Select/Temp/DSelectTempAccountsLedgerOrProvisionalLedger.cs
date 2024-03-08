using Inventory360Entity;
using DAL.Interface.Select.Temp;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Temp
{
    public class DSelectTempAccountsLedgerOrProvisionalLedger : ISelectTempAccountsLedgerOrProvisionalLedger
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTempAccountsLedgerOrProvisionalLedger(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Temp_AccountsLedgerOrProvisionalLedger> SelectTempAccountsLedgerOrProvisionalLedgerAll()
        {
            return _db.Temp_AccountsLedgerOrProvisionalLedger
                .Where(x => x.CompanyId == _companyId);
        }
    }
}