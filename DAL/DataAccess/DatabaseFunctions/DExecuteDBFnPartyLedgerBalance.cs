using Inventory360Entity;
using DAL.Interface.DatabaseFunctions;
using System;
using System.Data.SqlClient;
using System.ServiceModel;

namespace DAL.DataAccess.DatabaseFunctions
{
    public class DExecuteDBFnPartyLedgerBalance : IExecuteDBFnPartyLedgerBalance
    {
        private Inventory360Entities _db;
        private long _companyId;
        private long _customerId;
        private long _supplierId;
        private int _currencyLevel;
        private DateTime _toDate;

        public DExecuteDBFnPartyLedgerBalance(int currencyLevel, long customerId, long supplierId, DateTime toDate, long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
            _currencyLevel = currencyLevel;
            _customerId = customerId;
            _supplierId = supplierId;
            _toDate = toDate;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public decimal ExecuteDBFnPartyLedgerBalance()
        {
            try
            {
                decimal closingBalance = _db.Database.SqlQuery<decimal>("SELECT ClosingBalance = [dbo].[fn_CalculateOpeningBalanceForPartyLedger] (@currencyLevel, @customerId, @supplierId, @companyId, @toDate)", new SqlParameter("@currencyLevel", _currencyLevel), new SqlParameter("@customerId", _customerId), new SqlParameter("@supplierId", _supplierId), new SqlParameter("@companyId", _companyId), new SqlParameter("@toDate", _toDate)).FirstAsync().Result;
                return closingBalance;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}