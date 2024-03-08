using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Insert.Setup;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Setup
{
    public class DInsertSetupColor : IInsertSetupColor
    {
        private Inventory360Entities _db;
        private Setup_Color _entity;

        public DInsertSetupColor(CommonSetupColor entity)
        {
            _db = new Inventory360Entities();
            _entity = new Setup_Color
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
        public bool InsertColor()
        {
            try
            {
                _db.Setup_Color.Add(_entity);
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
