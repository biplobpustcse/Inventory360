using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Update.Setup;
using System;
using System.Data.Entity;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Setup
{
    public class DUpdateSetupAccountsControl : IUpdateSetupAccountsControl
    {
        private Inventory360Entities _db;
        private Setup_AccountsControl _findEntity;

        public DUpdateSetupAccountsControl(CommonSetupAccountsControl entity)
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;

            // Initialize value
            _findEntity = _db.Setup_AccountsControl.Find(entity.AccountsControlId);
            _findEntity.AccountsSubGroupId = entity.AccountsSubGroupId;
            _findEntity.Name = entity.ControlName;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateAccountsControl()
        {
            try
            {
                _db.Entry(_findEntity).State = EntityState.Modified;
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
