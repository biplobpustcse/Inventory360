using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Insert.Setup;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Setup
{
    public class DInsertSetupRelatedProduct : IInsertSetupRelatedProduct
    {
        private Inventory360Entities _db;
        private Setup_RelatedProduct _entity;

        public DInsertSetupRelatedProduct(CommonSetupRelatedProduct entity)
        {
            _db = new Inventory360Entities();
            _entity = new Setup_RelatedProduct
            {
                ProductId = entity.ProductId,
                RelatedOrSpareProductId = entity.RelatedOrSpareProductId,
                Type = entity.Type,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertRelatedProduct()
        {
            try
            {
                _db.Setup_RelatedProduct.Add(_entity);
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