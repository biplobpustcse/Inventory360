using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Insert.Setup;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Setup
{
    public class DInsertSetupProduct : IInsertSetupProduct
    {
        private Inventory360Entities _db;
        private Setup_Product _entity;

        public DInsertSetupProduct(CommonSetupProduct entity)
        {
            _db = new Inventory360Entities();
            _entity = new Setup_Product
            {
                ProductId = entity.ProductId,
                Code = entity.Code,
                ProductGroupId = entity.ProductGroupId,
                ProductSubGroupId = entity.ProductSubGroupId,
                ProductCategoryId = entity.ProductCategoryId,
                BrandId = entity.BrandId,
                Model = entity.Model,
                Name = entity.Name,
                ShortSpecification = entity.ShortSpecification,
                WebName = entity.WebName,
                SerialAvailable = entity.SerialAvailable,
                IsActive = true,
                PrimaryUnitTypeId = entity.PrimaryUnitTypeId,
                SecondaryUnitTypeId = entity.SecondaryUnitTypeId,
                TertiaryUnitTypeId = entity.TertiaryUnitTypeId,
                SecondaryConversionRatio = entity.SecondaryConversionRatio,
                TertiaryConversionRatio = entity.TertiaryConversionRatio,
                ReorderPoint = entity.ReorderPoint,
                MinStockQuantity = entity.MinStockQuantity,
                ReturnAllowedDays = entity.ReturnAllowedDays,
                CreditNoteAllowedDays = entity.CreditNoteAllowedDays,
                DebitNoteAllowedDays = entity.DebitNoteAllowedDays,
                IsStockMaintain = entity.IsStockMaintain,
                IsSaleable = entity.IsSaleable,
                ProductType = entity.ProductType,
                ProductCondition = entity.StockType,
                AccountsId = entity.AccountsId == 0 ? null : entity.AccountsId,
                IsWarrantyAvailable = entity.IsWarrantyAvailable,
                IsLifeTimeWarranty = entity.IsLifeTimeWarranty,
                WarrantyDays = entity.WarrantyDays,
                AdditionalWarrantyDays = entity.AdditionalWarrantyDays,
                IsServiceWarranty = entity.IsServiceWarranty,
                ServiceWarrantyDays = entity.ServiceWarrantyDays,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertProduct()
        {
            try
            {
                _db.Setup_Product.Add(_entity);
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