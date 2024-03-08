using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Insert.Setup;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Setup
{
    public class DInsertSetupPriceType : IInsertSetupPriceType
    {
        private Inventory360Entities _db;
        private Setup_PriceType _entity;

        public DInsertSetupPriceType(CommonSetupPriceType entity)
        {
            _db = new Inventory360Entities();
            _entity = new Setup_PriceType
            {
                Code = entity.Code,
                Name = entity.Name,
                IsDetailPrice = entity.IsDetailPrice,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertPriceType()
        {
            try
            {
                _db.Setup_PriceType.Add(_entity);
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