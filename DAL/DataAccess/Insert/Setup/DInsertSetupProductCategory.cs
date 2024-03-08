using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Insert.Setup;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Setup
{
    public class DInsertSetupProductCategory : IInsertSetupProductCategory
    {
        private Inventory360Entities _db;
        private Setup_ProductCategory _entity;

        public DInsertSetupProductCategory(CommonSetupProductCategory entity)
        {
            _db = new Inventory360Entities();
            _entity = new Setup_ProductCategory
            {
                ProductCategoryId = entity.ProductCategoryId,
                Code = entity.Code,
                Name = entity.Name,
                ProductGroupId = entity.ProductGroupId,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertProductCategory()
        {
            try
            {
                _db.Setup_ProductCategory.Add(_entity);
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