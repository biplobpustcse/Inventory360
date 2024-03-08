using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Update.Setup;
using System;
using System.Data.Entity;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Setup
{
    public class DUpdateSetupProductDimension : IUpdateSetupProductDimension
    {
        private Inventory360Entities _db;
        private Setup_ProductDimension _findEntity;

        public DUpdateSetupProductDimension(CommonSetupProductDimension entity)
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;

            // Initialize value
            _findEntity = _db.Setup_ProductDimension.Find(entity.ProductDimensionId);
            _findEntity.MeasurementId = entity.MeasurementId == 0 ? null : entity.MeasurementId;
            _findEntity.SizeId = entity.SizeId == 0 ? null : entity.SizeId;
            _findEntity.StyleId = entity.StyleId == 0 ? null : entity.StyleId;
            _findEntity.ColorId = entity.ColorId == 0 ? null : entity.ColorId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateProductDimension()
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