using Inventory360Entity;
using DAL.Interface.Delete.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Delete.Stock
{
    public class DDeleteTaskCurrentStockSerial : IDeleteTaskCurrentStockSerial
    {
        private Inventory360Entities _db;

        public DDeleteTaskCurrentStockSerial()
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool DeleteCurrentStockSerial(List<Guid> serialListIds)
        {
            try
            {
                _db.Stock_CurrentStockSerial
                    .RemoveRange(
                        _db.Stock_CurrentStockSerial
                        .Where(x => serialListIds.Contains(x.CurrentStockSerialId)
                        )
                    );

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