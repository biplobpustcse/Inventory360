using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Insert.Setup;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Setup
{
    public class DInsertSetupAccountsSubsidiary : IInsertSetupAccountsSubsidiary
    {
        private Inventory360Entities _db;
        private Setup_AccountsSubsidiary _entity;

        public DInsertSetupAccountsSubsidiary(CommonSetupAccountsSubsidiary entity)
        {
            _db = new Inventory360Entities();
            _entity = new Setup_AccountsSubsidiary
            {
                AccountsControlId = entity.AccountsControlId,
                Name = entity.SubsidiaryName,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertAccountsSubsidiary()
        {
            try
            {
                _db.Setup_AccountsSubsidiary.Add(_entity);
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
