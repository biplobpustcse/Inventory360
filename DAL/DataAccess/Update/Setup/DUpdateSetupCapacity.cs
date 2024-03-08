using Inventory360Entity;
using DAL.Interface.Update.Setup;
using System;
using System.Data.Entity;
using System.ServiceModel;
using Inventory360DataModel.Setup;

namespace DAL.DataAccess.Update.Setup
{
    public class DUpdateSetupCapacity : IUpdateSetupCapacity
    {
        private Inventory360Entities _db;
        private Setup_Capacity _findEntity;

        public DUpdateSetupCapacity(CommonSetupCapacity entity)
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;

            // Initialize value
            _findEntity = _db.Setup_Capacity.Find(entity.CapacityId);
            _findEntity.Code = entity.Code;
            _findEntity.Name = entity.Name;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateCapacity()
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