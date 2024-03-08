using Inventory360Entity;
using DAL.Interface.DatabaseFunctions;
using System;
using System.Data.SqlClient;
using System.ServiceModel;

namespace DAL.DataAccess.DatabaseFunctions
{
    public class DExecuteDBFnCurrencyLevel : IExecuteDBFnCurrencyLevel
    {
        private Inventory360Entities _db;
        private long _companyId;
        private string _currency;

        public DExecuteDBFnCurrencyLevel(string currency, long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
            _currency = currency;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public int ExecuteDBFnCurrentyLevel()
        {
            try
            {
                int currencyLevel = _db.Database.SqlQuery<int>("SELECT CurrencyLevel = [dbo].[fn_CurrencyLevel] (@companyId, @currenty)", new SqlParameter("@companyId", _companyId), new SqlParameter("@currenty", _currency)).FirstAsync().Result;
                return currencyLevel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}