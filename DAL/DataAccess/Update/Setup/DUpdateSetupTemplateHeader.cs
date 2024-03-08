using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Update.Setup;
using System;
using System.Data.Entity;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Setup
{
    public class DUpdateSetupTemplateHeader : IUpdateSetupTemplateHeader
    {
        private Inventory360Entities _db;
        private Setup_TemplateHeader _findEntity;

        public DUpdateSetupTemplateHeader(CommonSetupTemplateHeader entity)
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;

            // Initialize value
            _findEntity = _db.Setup_TemplateHeader.Find(entity.TemplateHeaderId);
            _findEntity.Name = entity.Name;
            _findEntity.Description = entity.Description;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateTemplateHeader()
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