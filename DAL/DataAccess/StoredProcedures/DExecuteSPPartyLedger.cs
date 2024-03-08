using Inventory360Entity;
using DAL.Interface.StoredProcedures;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.StoredProcedures
{
    public class DExecuteSPPartyLedger : IExecuteSPPartyLedger
    {
        private Inventory360Entities _db;
        private long _companyId;
        private DateTime _dateFrom;
        private DateTime _dateTo;
        private long _entryBy;
        private long _customerId;
        private string _currency;

        public DExecuteSPPartyLedger(string currency, long entryBy, long companyId, DateTime dateFrom, DateTime dateTo, long customerId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
            _dateFrom = dateFrom;
            _dateTo = dateTo;
            _entryBy = entryBy;
            _currency = currency;
            _customerId = customerId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public void ExecuteSPPartyLedger()
        {
            _db.Database.ExecuteSqlCommand("EXEC dbo.SP_PartyLedger @Currency = N'" + _currency + "', @CompanyId = " + _companyId + ", @DateFrom = '" + _dateFrom + "', @DateTo = '" + _dateTo + "', @CustomerId = " + _customerId + ", @EntryBy = " + _entryBy + " ");
        }
    }
}