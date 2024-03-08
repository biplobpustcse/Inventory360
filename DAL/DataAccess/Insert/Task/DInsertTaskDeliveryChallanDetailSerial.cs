using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskDeliveryChallanDetailSerial : IInsertTaskDeliveryChallanDetailSerial
    {
        private Inventory360Entities _db;
        private Task_DeliveryChallanDetailSerial _entity;

        public DInsertTaskDeliveryChallanDetailSerial(CommonTaskProductSerial entity)
        {
            _db = new Inventory360Entities();
            _entity = new Task_DeliveryChallanDetailSerial
            {
                ChallanDetailSerialId = Guid.NewGuid(),
                ChallanDetailId = entity.PrimaryId,
                Serial = entity.Serial,
                AdditionalSerial = entity.AdditionalSerial
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertDeliveryChallanDetailSerial()
        {
            try
            {
                _db.Task_DeliveryChallanDetailSerial.Add(_entity);
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