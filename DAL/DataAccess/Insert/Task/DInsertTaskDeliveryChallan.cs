using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskDeliveryChallan : IInsertTaskDeliveryChallan
    {
        private Inventory360Entities _db;
        private Task_DeliveryChallan _entity;

        public DInsertTaskDeliveryChallan(CommonTaskDeliveryChallan entity)
        {
            _db = new Inventory360Entities();
            _entity = new Task_DeliveryChallan
            {
                ChallanId = entity.ChallanId,
                ChallanNo = entity.ChallanNo,
                ChallanDate = entity.ChallanDate + DateTime.Now.TimeOfDay,
                CustomerId = entity.CustomerId,
                BuyerId = entity.BuyerId == 0 ? null : entity.BuyerId,
                SalesOrderId = entity.SalesOrderId,
                DeliveryFromId = entity.DeliveryFromId,
                DeliveryPlace = entity.DeliveryPlace,
                ContactPerson = entity.ContactPerson,
                ContactPersonNo = entity.ContactPersonNo,
                TransportId = entity.TransportId == 0 ? null : entity.TransportId,
                TransportTypeId = entity.TransportTypeId == 0 ? null : entity.TransportTypeId,
                VehicleNo = entity.VehicalNo,
                DriverName = entity.DriverName,
                DriverContactNo = entity.DriverContactNo,
                Approved = "N",
                LocationId = entity.LocationId,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertDeliveryChallan()
        {
            try
            {
                _db.Task_DeliveryChallan.Add(_entity);
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