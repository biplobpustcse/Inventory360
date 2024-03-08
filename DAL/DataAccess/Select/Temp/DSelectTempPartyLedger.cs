using Inventory360Entity;
using DAL.Interface.Select.Temp;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Temp
{
    public class DSelectTempPartyLedger : ISelectTempPartyLedger
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTempPartyLedger(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Temp_PartyLedger> SelectTempPartyLedgerAll()
        {
            return _db.Temp_PartyLedger
                .Where(x => x.CompanyId == _companyId);
        }
    }
}