using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Insert.Setup;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Setup
{
    public class DInsertSetupConvertionRatioDetail : IInsertSetupConvertionRatioDetail
    {
        private Inventory360Entities _db;
        private Setup_ConvertionRatioDetail _entity;

        public DInsertSetupConvertionRatioDetail(CommonSetupConvertionRatioDetail entity)
        {
            _db = new Inventory360Entities();
            _entity = new Setup_ConvertionRatioDetail
            {
                ConvertionRatioDetailId = entity.ConvertionRatioDetailId,
                ConvertionRatioId = entity.ConvertionRatioId,
                ProductFor = entity.ProductFor,
                ProductId = entity.ProductId,
                ProductDimensionId = entity.ProductDimensionId,
                UnitTypeId = entity.UnitTypeId,
                Quantity = entity.Quantity,
                Remarks = entity.Remarks
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertConvertionRatioDetail()
        {
            try
            {
                _db.Setup_ConvertionRatioDetail.Add(_entity);
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