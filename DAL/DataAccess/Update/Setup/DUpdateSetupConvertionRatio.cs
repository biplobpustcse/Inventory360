using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Update.Setup;
using System;
using System.Data.Entity;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Setup
{
    public class DUpdateSetupConvertionRatio : IUpdateSetupConvertionRatio
    {
        private Inventory360Entities _db;
        private Setup_ConvertionRatio _findEntity;

        public DUpdateSetupConvertionRatio(CommonSetupConvertionRatio entity)
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;

            // Initialize value
            _findEntity = _db.Setup_ConvertionRatio.Find(entity.ConvertionRatioId);
            _findEntity.RatioTitle = entity.RatioTitle;
            _findEntity.Description = entity.Description;
            _findEntity.EntryBy = entity.EntryBy;
            _findEntity.EntryDate = DateTime.Now;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateConvertionRatio()
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