using Inventory360Entity;
using DAL.Interface.StoredProcedures;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.StoredProcedures
{
    public class DExecuteSPTrialBalance : IExecuteSPTrialBalance
    {
        private Inventory360Entities _db;
        private long _companyId;
        private long _entryBy;
        private bool _reportType;
        private string _currency;
        private string _accLevel;
        private long _groupId;
        private long _subGroupId;
        private long _controlId;
        private long _subsidiaryId;
        private string _reportFor;
        private DateTime _dateFrom;
        private DateTime _dateTo;

        public DExecuteSPTrialBalance(long companyId, long entryBy, bool type, string currency, string accLevel, long groupId, long subGroupId, long controlId, long subsidiaryId, string reportFor, DateTime dateFrom, DateTime dateTo)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
            _entryBy = entryBy;
            _reportType = type;
            _currency = currency;
            _accLevel = accLevel;
            _groupId = groupId;
            _subGroupId = subGroupId;
            _controlId = controlId;
            _subsidiaryId = subsidiaryId;
            _reportFor = reportFor;
            _dateFrom = dateFrom;
            _dateTo = dateTo;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public void ExecuteSPTrialBalance()
        {
            _db.Database.ExecuteSqlCommand("EXEC dbo.SP_TrialBalance @CompanyId = " + _companyId + ", @EntryBy = " + _entryBy + ", @ReportType = " + _reportType + ", @Currency = N'" + _currency + "', @AccountsLevel = N'" + _accLevel + "', @GroupId = " + _groupId + ", @SubGroupId = " + _subGroupId + ", @ControlId = " + _controlId + ", @SubsidiaryId = " + _subsidiaryId + ", @ReportFor = N'" + _reportFor + "', @DateFrom = '" + _dateFrom + "', @DateTo = '" + _dateTo + "'");
        }
    }
}