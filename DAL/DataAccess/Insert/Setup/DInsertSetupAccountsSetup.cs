using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Insert.Setup;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Setup
{
    public class DInsertSetupAccountsSetup : DInsertSetupAccountsSetupBase, IInsertSetupAccountsSetup
    {
        public DInsertSetupAccountsSetup(CommonSetupAccountsSetup entity)
        {
            _entity = new Setup_Accounts
            {
                AccountsSubsidiaryId = entity.AccountsSubsidiaryId,
                Name = entity.AccountsName,
                CategorizationId = entity.CategorizationId,
                OpeningDate = GetCompanyOpeningDate(entity.CompanyId),
                OpeningBalance = entity.OpeningBalance,
                BalanceType = GetBalanceTypeByAccountsGroup(entity.CompanyId, entity.AccountsGroupId),
                LocationId = entity.LocationId,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertAccountsSetup()
        {
            try
            {
                _db.Setup_Accounts.Add(_entity);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}