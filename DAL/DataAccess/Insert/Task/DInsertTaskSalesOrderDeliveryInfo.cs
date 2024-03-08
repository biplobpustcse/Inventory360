using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskSalesOrderDeliveryInfo : IInsertTaskSalesOrderDeliveryInfo
    {
        private Inventory360Entities _db;
        private Task_SalesOrderDeliveryInfo _entity;

        public DInsertTaskSalesOrderDeliveryInfo(Guid salesOrderId, string deliveryPlace, string contactPerson, string contactPersonNo, long? transportId, long? transportTypeId, string vehicleNo, string driverName, string driverContactNo)
        {
            _db = new Inventory360Entities();
            _entity = new Task_SalesOrderDeliveryInfo
            {
                DeliveryInfoId = Guid.NewGuid(),
                SalesOrderId = salesOrderId,
                DeliveryPlace = deliveryPlace,
                ContactPerson = contactPerson,
                ContactPersonNo = contactPersonNo,
                TransportId = transportId == 0 ? null : transportId,
                TransportTypeId = transportTypeId == 0 ? null : transportTypeId,
                VehicleNo = vehicleNo,
                DriverName = driverName,
                DriverContactNo = driverContactNo
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertSalesOrderDeliveryInfo()
        {
            try
            {
                _db.Task_SalesOrderDeliveryInfo.Add(_entity);
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