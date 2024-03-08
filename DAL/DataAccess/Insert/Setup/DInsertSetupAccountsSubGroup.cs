using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Insert.Setup;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Setup
{
    public class DInsertSetupAccountsSubGroup : IInsertSetupAccountsSubGroup
    {
        private Inventory360Entities _db;
        private Setup_AccountsSubGroup _entity;

        public DInsertSetupAccountsSubGroup(CommonSetupAccountsSubGroup entity)
        {
            _db = new Inventory360Entities();
            _entity = new Setup_AccountsSubGroup
            {
                AccountsGroupId = entity.AccountsGroupId,
                Name = entity.SubGroupName,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertAccountsSubGroup()
        {
            try
            {
                _db.Setup_AccountsSubGroup.Add(_entity);
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
