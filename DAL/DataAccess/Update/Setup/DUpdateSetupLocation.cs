using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Update.Setup;
using System;
using System.Data.Entity;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Setup
{
    public class DUpdateSetupLocation : IUpdateSetupLocation
    {
        private Inventory360Entities _db;
        private Setup_Location _findEntity;

        public DUpdateSetupLocation(CommonSetupLocation entity)
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;

            // Initialize value
            _findEntity = _db.Setup_Location.Find(entity.LocationId);
            _findEntity.Code = entity.Code;
            _findEntity.Name = entity.Name;
            _findEntity.Address = entity.Address;
            _findEntity.ContactNo = entity.ContactNo;
            _findEntity.InChargeId = entity.InChargeId;
            _findEntity.DefaultCurrency = entity.DefaultCurrency;
            _findEntity.IsWareHouse = entity.IsWareHouse;
            _findEntity.MasterLocationId = (!entity.IsWareHouse ? null : entity.MasterLocationId);              // if warehouse is not checked then master location must null
            _findEntity.IsLoginLocation = (entity.IsWareHouse ? "N" : (entity.IsLoginLocation ? "Y" : "N"));    // if warehouse is checked then saved location is not login location
            _findEntity.IsPortLocation = entity.IsPortLocation;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateLocation()
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