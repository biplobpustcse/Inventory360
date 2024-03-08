using Inventory360Entity;
using DAL.Interface.Insert.Stock;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Stock
{
    public class DInsertStockCurrentStockSerial : IInsertStockCurrentStockSerial
    {
        private Inventory360Entities _db;
        private Stock_CurrentStockSerial _entity;

        public DInsertStockCurrentStockSerial(Guid stockId, string serial, string additionalSerial)
        {
            _db = new Inventory360Entities();
            _entity = new Stock_CurrentStockSerial
            {
                CurrentStockSerialId = Guid.NewGuid(),
                CurrentStockId = stockId,
                Serial = serial,
                AdditionalSerial = additionalSerial
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertCurrentStockSerial()
        {
            try
            {
                _db.Stock_CurrentStockSerial.Add(_entity);
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