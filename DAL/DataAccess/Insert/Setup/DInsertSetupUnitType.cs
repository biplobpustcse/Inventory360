using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Insert.Setup;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Setup
{
    public class DInsertSetupUnitType : IInsertSetupUnitType
    {
        private Inventory360Entities _db;
        private Setup_UnitType _entity;

        public DInsertSetupUnitType(CommonSetupUnitType entity)
        {
            _db = new Inventory360Entities();
            _entity = new Setup_UnitType
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
        public bool InsertUnitType()
        {
            try
            {
                _db.Setup_UnitType.Add(_entity);
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
