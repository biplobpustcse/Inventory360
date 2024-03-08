using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskConvertionDetailSerial : IInsertTaskConvertionDetailSerial
    {
        private Inventory360Entities _db;
        private Task_ConvertionDetailSerial _entity;

        public DInsertTaskConvertionDetailSerial(CommonTaskConvertionDetailSerial entity)
        {
            _db = new Inventory360Entities();
            _entity = new Task_ConvertionDetailSerial
            {
                ConvertionDetailId = entity.ConvertionDetailId,
                ConvertionDetailSerialId = entity.ConvertionDetailSerialId,
                Serial = entity.Serial,
                AdditionalSerial = entity.AdditionalSerial
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertConvertionDetailSerial()
        {
            try
            {
                _db.Task_ConvertionDetailSerial.Add(_entity);
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