using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Update.Setup;
using System;
using System.Data.Entity;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Setup
{
    public class DUpdateSetupBrand : IUpdateSetupBrand
    {
        private Inventory360Entities _db;
        private Setup_Brand _findEntity;

        public DUpdateSetupBrand(CommonSetupBrand entity)
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;

            // Initialize value
            _findEntity = _db.Setup_Brand.Find(entity.BrandId);
            _findEntity.Code = entity.Code;
            _findEntity.Name = entity.Name;
            _findEntity.EditedBy = entity.EntryBy;
            _findEntity.EditedDate = DateTime.Now;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateBrand()
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
