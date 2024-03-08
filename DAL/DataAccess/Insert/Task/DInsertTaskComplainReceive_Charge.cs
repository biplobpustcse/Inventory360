using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskComplainReceive_Charge : IInsertTaskComplainReceive_Charge
    {
        private Inventory360Entities _db;
        //private Task_ComplainReceiveDetail_Charge _entity;
        private Task_ComplainReceive_Charge _entity;

        public DInsertTaskComplainReceive_Charge(CommonComplainReceive_Charge entity)
        {
            _db = new Inventory360Entities();
            _entity = new Task_ComplainReceive_Charge
            {
                ReceiveId = entity.ReceiveId,
                ReceiveChargeId = entity.ReceiveChargeId,               
                ChargeEventId = entity.ChargeEventId,
                ChargeAmount = entity.ChargeAmount,
                Charge1Amount = entity.Charge1Amount,
                Charge2Amount = entity.Charge2Amount
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertComplainReceiveDetail_Charge()
        {
            try
            {
                _db.Task_ComplainReceive_Charge.Add(_entity);
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