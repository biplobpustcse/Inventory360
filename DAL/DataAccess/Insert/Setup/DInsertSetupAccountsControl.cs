using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Insert.Setup;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Setup
{
    public class DInsertSetupAccountsControl : IInsertSetupAccountsControl
    {
        private Inventory360Entities _db;
        private Setup_AccountsControl _entity;

        public DInsertSetupAccountsControl(CommonSetupAccountsControl entity)
        {
            _db = new Inventory360Entities();
            _entity = new Setup_AccountsControl
            {
                AccountsSubGroupId = entity.AccountsSubGroupId,
                Name = entity.ControlName,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertAccountsControl()
        {
            try
            {
                _db.Setup_AccountsControl.Add(_entity);
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
