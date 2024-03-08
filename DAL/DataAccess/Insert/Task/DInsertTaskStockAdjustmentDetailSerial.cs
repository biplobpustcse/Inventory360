using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskStockAdjustmentDetailSerial : IInsertTaskStockAdjustmentDetailSerial
    {
        private Inventory360Entities _db;
        private Task_StockAdjustmentDetailSerial _entity;

        public DInsertTaskStockAdjustmentDetailSerial(CommonTaskProductSerial entity)
        {
            _db = new Inventory360Entities();
            _entity = new Task_StockAdjustmentDetailSerial
            {
                AdjustmentDetailSerialId = Guid.NewGuid(),
                AdjustmentDetailId = entity.PrimaryId,
                Serial = entity.Serial,
                AdditionalSerial = entity.AdditionalSerial
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertStockAdjustmentDetailSerial()
        {
            try
            {
                _db.Task_StockAdjustmentDetailSerial.Add(_entity);
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