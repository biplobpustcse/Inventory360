using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Insert.Setup;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Setup
{
    public class DInsertSetupConvertionRatio : IInsertSetupConvertionRatio
    {
        private Inventory360Entities _db;
        private Setup_ConvertionRatio _entity;

        public DInsertSetupConvertionRatio(CommonSetupConvertionRatio entity)
        {
            _db = new Inventory360Entities();
            _entity = new Setup_ConvertionRatio
            {
                ConvertionRatioId = entity.ConvertionRatioId,
                RatioNo = entity.RatioNo,
                RatioTitle = entity.RatioTitle,
                RatioDate = entity.RatioDate,
                Description = entity.Description,
                Approved = entity.Approved,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertConvertionRatio()
        {
            try
            {
                _db.Setup_ConvertionRatio.Add(_entity);
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