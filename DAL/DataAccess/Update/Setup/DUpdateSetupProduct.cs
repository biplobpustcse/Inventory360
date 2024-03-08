using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Update.Setup;
using System;
using System.Data.Entity;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Setup
{
    public class DUpdateSetupProduct : IUpdateSetupProduct
    {
        private Inventory360Entities _db;
        private Setup_Product _findEntity;

        public DUpdateSetupProduct(CommonSetupProduct entity)
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;

            // Initialize value
            _findEntity = _db.Setup_Product.Find(entity.ProductId);
            _findEntity.Code = entity.Code;
            _findEntity.ProductGroupId = entity.ProductGroupId;
            _findEntity.ProductSubGroupId = entity.ProductSubGroupId;
            _findEntity.ProductCategoryId = entity.ProductCategoryId;
            _findEntity.BrandId = entity.BrandId;
            _findEntity.Model = entity.Model;
            _findEntity.Name = entity.Name;
            _findEntity.ShortSpecification = entity.ShortSpecification;
            _findEntity.WebName = entity.WebName;
            _findEntity.SerialAvailable = entity.SerialAvailable;
            _findEntity.IsActive = entity.IsActive;
            _findEntity.PrimaryUnitTypeId = entity.PrimaryUnitTypeId;
            _findEntity.SecondaryUnitTypeId = entity.SecondaryUnitTypeId;
            _findEntity.TertiaryUnitTypeId = entity.TertiaryUnitTypeId;
            _findEntity.SecondaryConversionRatio = entity.SecondaryConversionRatio;
            _findEntity.TertiaryConversionRatio = entity.TertiaryConversionRatio;
            _findEntity.ReorderPoint = entity.ReorderPoint;
            _findEntity.MinStockQuantity = entity.MinStockQuantity;
            _findEntity.ReturnAllowedDays = entity.ReturnAllowedDays;
            _findEntity.CreditNoteAllowedDays = entity.CreditNoteAllowedDays;
            _findEntity.DebitNoteAllowedDays = entity.DebitNoteAllowedDays;
            _findEntity.IsStockMaintain = entity.IsStockMaintain;
            _findEntity.IsSaleable = entity.IsSaleable;
            _findEntity.ProductType = entity.ProductType;
            _findEntity.ProductCondition = entity.StockType;
            _findEntity.AccountsId = entity.AccountsId == 0 ? null : entity.AccountsId;
            _findEntity.IsWarrantyAvailable = entity.IsWarrantyAvailable;
            _findEntity.IsLifeTimeWarranty = entity.IsLifeTimeWarranty;
            _findEntity.WarrantyDays = entity.WarrantyDays;
            _findEntity.AdditionalWarrantyDays = entity.AdditionalWarrantyDays;
            _findEntity.IsServiceWarranty = entity.IsServiceWarranty;
            _findEntity.ServiceWarrantyDays = entity.ServiceWarrantyDays;
            _findEntity.EditedBy = entity.EntryBy;
            _findEntity.EditedDate = DateTime.Now;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateProduct()
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