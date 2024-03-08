using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Update.Setup;
using System;
using System.Data.Entity;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Setup
{
    public class DUpdateSetupTransportType : IUpdateSetupTransportType
    {
        private Inventory360Entities _db;
        private Setup_TransportType _findEntity;

        public DUpdateSetupTransportType(CommonSetupTransportType entity)
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;

            // Initialize value
            _findEntity = _db.Setup_TransportType.Find(entity.TransportTypeId);
            _findEntity.Name = entity.Name;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateTransportType()
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