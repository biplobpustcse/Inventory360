using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Insert.Setup;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Setup
{
    public class DInsertSetupProject : IInsertSetupProject
    {
        private Inventory360Entities _db;
        private Setup_Project _entity;

        public DInsertSetupProject(CommonSetupProject entity)
        {
            _db = new Inventory360Entities();
            _entity = new Setup_Project
            {
                Name = entity.Name,
                Description = entity.Description,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                IsActive = entity.IsActive,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertProject()
        {
            try
            {
                _db.Setup_Project.Add(_entity);
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