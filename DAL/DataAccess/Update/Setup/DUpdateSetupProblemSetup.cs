using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Update.Setup;
using System;
using System.Data.Entity;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Setup
{
    public class DUpdateSetupProblemSetup : IUpdateSetupProblemSetup
    {
        private Inventory360Entities _db;
        private Setup_Problem _findEntity;

        public DUpdateSetupProblemSetup(CommonSetupProblemSetup entity)
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;

            // Initialize value
            _findEntity = _db.Setup_Problem.Find(entity.ProblemId);
            _findEntity.OperationalEventId = entity.OperationalEventId;
            _findEntity.Name = entity.Name;
            _findEntity.EditedBy = entity.EntryBy;
            _findEntity.EditedDate = DateTime.Now;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateProblemSetup()
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