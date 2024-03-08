using Inventory360Entity;
using DAL.Interface.Update.Task;
using System;
using System.Data.Entity;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Task
{
    public class DUpdateTaskConvertionDetail : IUpdateTaskConvertionDetail
    {
        private Inventory360Entities _db;
        private Task_ConvertionDetail _findEntity;

        public DUpdateTaskConvertionDetail(Guid id)
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;

            // Initialize value
            _findEntity = _db.Task_ConvertionDetail.Find(id);
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateConvertionDetailReference(Guid? GoodsReceiveId, Guid? ImportedStockInId, long? SupplierId, string ReferenceNo, DateTime? ReferenceDate)
        {
            try
            {
                _findEntity.GoodsReceiveId = GoodsReceiveId;
                _findEntity.ImportedStockInId = ImportedStockInId;
                _findEntity.SupplierId = SupplierId;
                _findEntity.ReferenceNo = ReferenceNo;
                _findEntity.ReferenceDate = ReferenceDate;

                _db.Entry(_findEntity).State = EntityState.Modified;
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