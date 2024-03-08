using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertStockAdjustmentNos : IInsertStockAdjustmentNos
    {
        private Inventory360Entities _db;
        private Task_StockAdjustmentNos _entity;

        public DInsertStockAdjustmentNos(string adjustmentNo, long year, long companyId)
        {
            _db = new Inventory360Entities();
            _entity = new Task_StockAdjustmentNos
            {
                Id = Guid.NewGuid(),
                AdjustmentNo = adjustmentNo,
                Year = year,
                CompanyId = companyId
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertStockAdjustmentNos()
        {
            try
            {
                _db.Task_StockAdjustmentNos.Add(_entity);
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