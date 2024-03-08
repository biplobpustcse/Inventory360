using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskReplacementReceive_Charge : IInsertTaskReplacementReceive_Charge
    {
        private Inventory360Entities _db;
        //private Task_ComplainReceiveDetail_Charge _entity;
        private Task_ReplacementReceive_Charge _entity;

        public DInsertTaskReplacementReceive_Charge(CommonTaskReplacementReceive_Charge entity)
        {
            _db = new Inventory360Entities();
            _entity = new Task_ReplacementReceive_Charge
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
        public bool InsertReplacementReceive_Charge()
        {
            try
            {
                _db.Task_ReplacementReceive_Charge.Add(_entity);
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