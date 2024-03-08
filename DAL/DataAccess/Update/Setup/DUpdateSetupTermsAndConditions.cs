using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Update.Setup;
using System;
using System.Data.Entity;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Setup
{
    public class DUpdateSetupTermsAndConditions : IUpdateSetupTermsAndConditions
    {
        private Inventory360Entities _db;
        private Setup_TermsAndConditions _findEntity;

        public DUpdateSetupTermsAndConditions(CommonSetupTermsAndConditions entity, long operationalEventId)
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;

            // Initialize value
            _findEntity = _db.Setup_TermsAndConditions.Find(entity.TermsAndConditionsId);
            _findEntity.OperationalEventId = operationalEventId;
            _findEntity.TemplateHeaderId = entity.TemplateHeaderId;
            _findEntity.Detail = entity.Detail;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateTermsAndConditions()
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