using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Insert.Setup;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Setup
{
    public class DInsertSetupStyle : IInsertSetupStyle
    {
        private Inventory360Entities _db;
        private Setup_Style _entity;

        public DInsertSetupStyle(CommonSetupStyle entity)
        {
            _db = new Inventory360Entities();
            _entity = new Setup_Style
            {
                Code = entity.Code,
                Name = entity.Name,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertStyle()
        {
            try
            {
                _db.Setup_Style.Add(_entity);
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