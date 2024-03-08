using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Insert.Setup;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Setup
{
    public class DInsertSetupLocation : IInsertSetupLocation
    {
        private Inventory360Entities _db;
        private Setup_Location _entity;

        public DInsertSetupLocation(CommonSetupLocation entity)
        {
            _db = new Inventory360Entities();
            _entity = new Setup_Location
            {
                Code = entity.Code,
                Name = entity.Name,
                Address = entity.Address,
                ContactNo = entity.ContactNo,
                InChargeId = entity.InChargeId,
                DefaultCurrency = entity.DefaultCurrency,
                IsWareHouse = entity.IsWareHouse,
                MasterLocationId = (!entity.IsWareHouse ? null : entity.MasterLocationId),              // if warehouse is not checked then master location must null
                IsLoginLocation = (entity.IsWareHouse ? "N" : (entity.IsLoginLocation ? "Y" : "N")),    // if warehouse is checked then saved location is not login location
                IsPortLocation = entity.IsPortLocation,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertLocation()
        {
            try
            {
                _db.Setup_Location.Add(_entity);
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