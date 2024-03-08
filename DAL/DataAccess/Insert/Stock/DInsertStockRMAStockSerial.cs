using Inventory360Entity;
using DAL.Interface.Insert.Stock;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Stock
{
    public class DInsertStockRMAStockSerial : IInsertStockRMAStockSerial
    {
        private Inventory360Entities _db;
        private Stock_RMAStockSerial _entity;

        public DInsertStockRMAStockSerial(Guid stockId, string serial, string additionalSerial)
        {
            _db = new Inventory360Entities();
            _entity = new Stock_RMAStockSerial
            {
                RMAStockSerialId = Guid.NewGuid(),
                RMAStockId = stockId,
                Serial = serial,
                AdditionalSerial = additionalSerial
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertRMAStockSerial()
        {
            try
            {
                _db.Stock_RMAStockSerial.Add(_entity);
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