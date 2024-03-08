using Inventory360Entity;
using DAL.Interface.DatabaseFunctions;
using System;
using System.Data.SqlClient;
using System.ServiceModel;

namespace DAL.DataAccess.DatabaseFunctions
{
    public class DExecuteDBFnSupplierLedgerBalance : IExecuteDBFnSupplierLedgerBalance
    {
        private Inventory360Entities _db;
        private long _companyId;
        private long _supplierId;
        private int _currencyLevel;
        private DateTime _toDate;

        public DExecuteDBFnSupplierLedgerBalance(int currencyLevel, long supplierId, DateTime toDate, long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
            _currencyLevel = currencyLevel;
            _supplierId = supplierId;
            _toDate = toDate;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public decimal ExecuteDBFnSupplierLedgerBalance()
        {
            try
            {
                decimal closingBalance = _db.Database.SqlQuery<decimal>("SELECT ClosingBalance = [dbo].[fn_CalculateOpeningBalanceForSupplierLedger] (@currencyLevel, @supplierId, @companyId, @toDate)", new SqlParameter("@currencyLevel", _currencyLevel), new SqlParameter("@supplierId", _supplierId), new SqlParameter("@companyId", _companyId), new SqlParameter("@toDate", _toDate)).FirstAsync().Result;
                return closingBalance;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}