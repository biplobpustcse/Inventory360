using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Update.Setup;
using System;
using System.Data.Entity;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Setup
{
    public class DUpdateSetupCustomerGroup : IUpdateSetupCustomerGroup
    {
        private Inventory360Entities _db;
        private Setup_CustomerGroup _findEntity;

        public DUpdateSetupCustomerGroup(CommonSetupCustomerGroup entity)
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;

            // Initialize value
            _findEntity = _db.Setup_CustomerGroup.Find(entity.CustomerGroupId);
            _findEntity.Name = entity.Name;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateCustomerGroup()
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
