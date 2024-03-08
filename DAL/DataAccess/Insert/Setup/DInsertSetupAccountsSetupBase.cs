using Inventory360Entity;
using System;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Setup
{
    public class DInsertSetupAccountsSetupBase
    {
        protected Inventory360Entities _db;
        protected Setup_Accounts _entity;

        protected DInsertSetupAccountsSetupBase()
        {
            _db = new Inventory360Entities();
        }

        // Get accounts group wise balance type
        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        protected string GetBalanceTypeByAccountsGroup(long companyId, long accountsGroupId)
        {
            return _db.Setup_AccountsGroup
                .Where(x => x.CompanyId == companyId && x.AccountsGroupId == accountsGroupId)
                .Select(s => s.BalanceType)
                .FirstOrDefault();
        }

        // Get company's opening date
        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        protected DateTime GetCompanyOpeningDate(long companyId)
        {
            return _db.Setup_Company
                .Where(x => x.CompanyId == companyId)
                .Select(s => s.OpeningDate)
                .FirstOrDefault();
        }
    }
}
