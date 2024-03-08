using Inventory360Entity;
using DAL.Interface.StoredProcedures;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.StoredProcedures
{
    public class DExecuteSPSupplierLedger : IExecuteSPSupplierLedger
    {
        private Inventory360Entities _db;
        private long _companyId;
        private DateTime _dateFrom;
        private DateTime _dateTo;
        private long _entryBy;
        private long _supplierId;
        private string _currency;

        public DExecuteSPSupplierLedger(string currency, long entryBy, long companyId, DateTime dateFrom, DateTime dateTo, long supplierId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
            _dateFrom = dateFrom;
            _dateTo = dateTo;
            _entryBy = entryBy;
            _currency = currency;
            _supplierId = supplierId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public void ExecuteSPSupplierLedger()
        {
            _db.Database.ExecuteSqlCommand("EXEC dbo.SP_SupplierLedger @Currency = N'" + _currency + "', @CompanyId = " + _companyId + ", @DateFrom = '" + _dateFrom + "', @DateTo = '" + _dateTo + "', @SupplierId = " + _supplierId + ", @EntryBy = " + _entryBy + " ");
        }
    }
}