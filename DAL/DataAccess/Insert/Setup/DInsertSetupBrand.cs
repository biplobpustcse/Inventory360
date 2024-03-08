using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Insert.Setup;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Setup
{
    public class DInsertSetupBrand : IInsertSetupBrand
    {
        private Inventory360Entities _db;
        private Setup_Brand _entity;

        public DInsertSetupBrand(CommonSetupBrand entity)
        {
            _db = new Inventory360Entities();
            _entity = new Setup_Brand
            {
                BrandId = entity.BrandId,
                Code = entity.Code,
                Name = entity.Name,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertBrand()
        {
            try
            {
                _db.Setup_Brand.Add(_entity);
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
