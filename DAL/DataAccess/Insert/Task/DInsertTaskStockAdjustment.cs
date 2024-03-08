using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertStockAdjustment : IInsertStockAdjustment
    {
        private Inventory360Entities _db;
        private Task_StockAdjustment _entity;

        public DInsertStockAdjustment(CommonTaskStockAdjustment entity, decimal exchangeRate1, decimal exchangeRate2)
        {
            _db = new Inventory360Entities();
            _entity = new Task_StockAdjustment
            {
                AdjustmentId = entity.AdjustmentId,
                AdjustmentNo = entity.AdjustmentNo,
                AdjustmentDate = entity.AdjustmentDate + DateTime.Now.TimeOfDay,
                SelectedCurrency = entity.SelectedCurrency,
                Currency1Rate = exchangeRate1,
                Currency2Rate = exchangeRate2,
                Approved = "N",
                RequestedBy = entity.EmployeeId == 0 ? null : entity.EmployeeId,
                Remarks = entity.Remarks,
                LocationId = entity.LocationId,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertStockAdjustment()
        {
            try
            {
                _db.Task_StockAdjustment.Add(_entity);
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