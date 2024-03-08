using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Update.Setup;
using System;
using System.Data.Entity;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Setup
{
    public class DUpdateSetupConvertionRatioDetail : IUpdateSetupConvertionRatioDetail
    {
        private Inventory360Entities _db;
        private Setup_ConvertionRatioDetail _findEntity;

        public DUpdateSetupConvertionRatioDetail(CommonSetupConvertionRatioDetail entity)
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;

            // Initialize value
            _findEntity = _db.Setup_ConvertionRatioDetail.Find(entity.ConvertionRatioDetailId);
            _findEntity.ProductFor = entity.ProductFor;
            _findEntity.ProductId = entity.ProductId;
            _findEntity.ProductDimensionId = entity.ProductDimensionId;
            _findEntity.UnitTypeId = entity.UnitTypeId;
            _findEntity.Quantity = entity.Quantity;
            _findEntity.Remarks = entity.Remarks;            
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateConvertionRatioDetail()
        {
            try
            {
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