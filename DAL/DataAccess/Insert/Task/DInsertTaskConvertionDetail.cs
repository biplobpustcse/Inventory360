using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskConvertionDetail : IInsertTaskConvertionDetail
    {
        private Inventory360Entities _db;
        private Task_ConvertionDetail _entity;

        public DInsertTaskConvertionDetail(CommonTaskConvertionDetail entity)
        {
            _db = new Inventory360Entities();
            _entity = new Task_ConvertionDetail
            {
                ConvertionDetailId = entity.ConvertionDetailId,
                ConvertionId = entity.ConvertionId,
                ProductFor = entity.ProductFor,
                ProductId = entity.ProductId,
                ProductDimensionId = entity.ProductDimensionId,
                UnitTypeId = entity.UnitTypeId,
                PrimaryUnitTypeId = entity.PrimaryUnitTypeId,
                SecondaryUnitTypeId = entity.SecondaryUnitTypeId,
                TertiaryUnitTypeId = entity.TertiaryUnitTypeId,
                SecondaryConversionRatio = entity.SecondaryConversionRatio,
                TertiaryConversionRatio = entity.TertiaryConversionRatio,
                WareHouseId = entity.WareHouseId == 0 ? null : entity.WareHouseId,
                Quantity = entity.Quantity,
                Cost = entity.Cost,
                Cost1 = entity.Cost1,
                Cost2 = entity.Cost2,
                ReferenceNo = String.IsNullOrEmpty(entity.ReferenceNo)? "" : entity.ReferenceNo,
                ReferenceDate = entity.ReferenceDate,
                GoodsReceiveId = entity.GoodsReceiveId == Guid.Empty ? null : entity.GoodsReceiveId,
                ImportedStockInId = entity.ImportedStockInId == Guid.Empty ? null : entity.ImportedStockInId,
                SupplierId = entity.SupplierId == 0 ? null : entity.SupplierId
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertConvertionDetail()
        {
            try
            {
                _db.Task_ConvertionDetail.Add(_entity);
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