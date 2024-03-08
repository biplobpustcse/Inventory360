using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Insert.Setup;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Setup
{
    public class DInsertSetupDesignation : IInsertSetupDesignation
    {
        private Inventory360Entities _db;
        private Setup_Designation _entity;

        public DInsertSetupDesignation(CommonSetupDesignation entity)
        {
            _db = new Inventory360Entities();
            _entity = new Setup_Designation
            {
                Name = entity.Name,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertDesignation()
        {
            try
            {
                _db.Setup_Designation.Add(_entity);
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
