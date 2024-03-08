using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Update.Setup;
using System;
using System.Data.Entity;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Setup
{
    public class DUpdateSetupProject : IUpdateSetupProject
    {
        private Inventory360Entities _db;
        private Setup_Project _findEntity;

        public DUpdateSetupProject(CommonSetupProject entity)
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;

            // Initialize value
            _findEntity = _db.Setup_Project.Find(entity.ProjectId);
            _findEntity.Name = entity.Name;
            _findEntity.Description = entity.Description;
            _findEntity.StartDate = entity.StartDate;
            _findEntity.EndDate = entity.EndDate;
            _findEntity.IsActive = entity.IsActive;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateProject()
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