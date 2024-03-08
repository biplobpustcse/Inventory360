using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Update.Setup;
using System;
using System.Data.Entity;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Setup
{
    public class DUpdateSetupPriceType : IUpdateSetupPriceType
    {
        private Inventory360Entities _db;
        private Setup_PriceType _findEntity;

        public DUpdateSetupPriceType(CommonSetupPriceType entity)
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;

            // Initialize value
            _findEntity = _db.Setup_PriceType.Find(entity.PriceTypeId);
            _findEntity.Code = entity.Code;
            _findEntity.Name = entity.Name;
            _findEntity.IsDetailPrice = entity.IsDetailPrice;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdatePriceType()
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