using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.StoredProcedures;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.StoredProcedures
{
    public class DExecuteSPAccountsLedger : IExecuteSPAccountsLedger
    {
        private Inventory360Entities _db;
        private long _companyId;
        private DateTime _dateFrom;
        private DateTime _dateTo;
        private long _accGroupId;
        private long _accSubGroupId;
        private long _accControlId;
        private long _accSubsidiaryId;
        private long _accId;
        private long _entryBy;
        private string _reportType;
        private string _currency;

        public DExecuteSPAccountsLedger(string currency, string type, long entryBy, string radioId, long id, long companyId, DateTime dateFrom, DateTime dateTo)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
            _dateFrom = dateFrom;
            _dateTo = dateTo;
            _entryBy = entryBy;
            _reportType = type;
            _currency = currency;

            if (radioId == CommonEnum.AccountsTree.Group.ToString())
            {
                _accGroupId = id;
            }
            else if (radioId == CommonEnum.AccountsTree.SubGroup.ToString())
            {
                _accSubGroupId = id;
            }
            else if (radioId == CommonEnum.AccountsTree.Control.ToString())
            {
                _accControlId = id;
            }
            else if (radioId == CommonEnum.AccountsTree.Subsidiary.ToString())
            {
                _accSubsidiaryId = id;
            }
            else
            {
                _accId = id;
            }
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public void ExecuteSPAccountsLedgerOrProvisionalLedger()
        {
            _db.Database.ExecuteSqlCommand("EXEC dbo.SP_AccountsLedger @Currency = N'" + _currency + "', @CompanyId = " + _companyId + ", @DateFrom = '" + _dateFrom + "', @DateTo = '" + _dateTo + "', @AccGroupId = " + _accGroupId + ", @AccSubGroupId = " + _accSubGroupId + ", @AccControlId = " + _accControlId + ", @AccSubsidiaryId = " + _accSubsidiaryId + ", @AccId = " + _accId + ", @EntryBy = " + _entryBy + ", @ReportType = N'" + _reportType + "' ");
        }
    }
}