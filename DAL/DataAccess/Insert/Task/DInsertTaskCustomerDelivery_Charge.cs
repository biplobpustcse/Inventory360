using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskCustomerDelivery_Charge : IInsertTaskCustomerDelivery_Charge
    {
        private Inventory360Entities _db;
        //private Task_ComplainReceiveDetail_Charge _entity;
        private Task_CustomerDelivery_Charge _entity;

        public DInsertTaskCustomerDelivery_Charge(CommonTaskCustomerDelivery_Charge entity)
        {
            _db = new Inventory360Entities();
            _entity = new Task_CustomerDelivery_Charge
            {
                DeliveryId = entity.DeliveryId,
                DeliveryChargeId = entity.DeliveryChargeId,               
                ChargeEventId = entity.ChargeEventId,
                ChargeAmount = entity.ChargeAmount,
                Charge1Amount = entity.Charge1Amount,
                Charge2Amount = entity.Charge2Amount
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertCustomerDelivery_Charge()
        {
            try
            {
                _db.Task_CustomerDelivery_Charge.Add(_entity);
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