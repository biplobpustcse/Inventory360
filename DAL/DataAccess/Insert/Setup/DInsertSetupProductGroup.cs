using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Insert.Setup;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Setup
{
    public class DInsertSetupProductGroup : IInsertSetupProductGroup
    {
        private Inventory360Entities _db;
        private Setup_ProductGroup _entity;

        public DInsertSetupProductGroup(CommonSetupProductGroup entity)
        {
            _db = new Inventory360Entities();
            _entity = new Setup_ProductGroup
            {
                ProductGroupId = entity.ProductGroupId,
                Code = entity.Code,
                Name = entity.Name,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertProductGroup()
        {
            try
            {
                _db.Setup_ProductGroup.Add(_entity);
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