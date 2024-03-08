using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Update.Setup;
using System;
using System.Data.Entity;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Setup
{
    public class DUpdateSetupProductCategory : IUpdateSetupProductCategory
    {
        private Inventory360Entities _db;
        private Setup_ProductCategory _findEntity;

        public DUpdateSetupProductCategory(CommonSetupProductCategory entity)
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;

            // Initialize value
            _findEntity = _db.Setup_ProductCategory.Find(entity.ProductCategoryId);
            _findEntity.Code = entity.Code;
            _findEntity.Name = entity.Name;
            _findEntity.ProductGroupId = entity.ProductGroupId;
            _findEntity.EditedBy = entity.EntryBy;
            _findEntity.EditedDate = DateTime.Now;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateProductCategory()
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