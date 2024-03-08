using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Insert.Setup;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Setup
{
    public class DInsertSetupProductDimension : IInsertSetupProductDimension
    {
        private Inventory360Entities _db;
        private Setup_ProductDimension _entity;

        public DInsertSetupProductDimension(CommonSetupProductDimension entity)
        {
            _db = new Inventory360Entities();
            _entity = new Setup_ProductDimension
            {
                ProductId = entity.ProductId,
                MeasurementId = entity.MeasurementId == 0 ? null : entity.MeasurementId,
                SizeId = entity.SizeId == 0 ? null : entity.SizeId,
                StyleId = entity.StyleId == 0 ? null : entity.StyleId,
                ColorId = entity.ColorId == 0 ? null : entity.ColorId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertProductDimension()
        {
            try
            {
                _db.Setup_ProductDimension.Add(_entity);
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