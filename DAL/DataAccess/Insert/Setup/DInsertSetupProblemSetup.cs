using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Insert.Setup;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Setup
{
    public class DInsertSetupProblemSetup : IInsertSetupProblemSetup
    {
        private Inventory360Entities _db;
        private Setup_Problem _entity;

        public DInsertSetupProblemSetup(CommonSetupProblemSetup entity)
        {
            _db = new Inventory360Entities();
            _entity = new Setup_Problem
            {
                ProblemId = entity.ProblemId,
                Name = entity.Name,
                OperationalEventId = entity.OperationalEventId,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertProblemSetup()
        {
            try
            {
                _db.Setup_Problem.Add(_entity);
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