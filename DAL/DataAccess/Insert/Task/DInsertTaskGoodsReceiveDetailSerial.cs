using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskGoodsReceiveDetailSerial : IInsertTaskGoodsReceiveDetailSerial
    {
        private Inventory360Entities _db;
        private Task_GoodsReceiveDetailSerial _entity;

        public DInsertTaskGoodsReceiveDetailSerial(CommonTaskProductSerial entity)
        {
            _db = new Inventory360Entities();
            _entity = new Task_GoodsReceiveDetailSerial
            {
                ReceiveDetailSerialId = Guid.NewGuid(),
                ReceiveDetailId = entity.PrimaryId,
                Serial = entity.Serial,
                AdditionalSerial = entity.AdditionalSerial
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertGoodsReceiveDetailSerial()
        {
            try
            {
                _db.Task_GoodsReceiveDetailSerial.Add(_entity);
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