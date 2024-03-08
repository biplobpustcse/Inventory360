using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskDeliveryChallanDetail : IInsertTaskDeliveryChallanDetail
    {
        private Inventory360Entities _db;
        private Task_DeliveryChallanDetail _entity;

        public DInsertTaskDeliveryChallanDetail(CommonTaskDeliveryChallanDetail entity)
        {
            _db = new Inventory360Entities();
            _entity = new Task_DeliveryChallanDetail
            {
                ChallanDetailId = entity.ChallanDetailId,
                ChallanId = entity.ChallanId,
                WareHouseId = entity.WareHouseId == 0 ? null : entity.WareHouseId,
                ProductId = entity.ProductId,
                ProductDimensionId = entity.ProductDimensionId == 0 ? null : entity.ProductDimensionId,
                UnitTypeId = entity.UnitTypeId,
                PrimaryUnitTypeId = entity.PrimaryUnitTypeId,
                SecondaryUnitTypeId = entity.SecondaryUnitTypeId,
                TertiaryUnitTypeId = entity.TertiaryUnitTypeId,
                SecondaryConversionRatio = entity.SecondaryConversionRatio,
                TertiaryConversionRatio = entity.TertiaryConversionRatio,
                Quantity = entity.Quantity,
                Price = entity.Price,
                Price1 = entity.Price1,
                Price2 = entity.Price2,
                Discount = entity.Discount,
                Discount1 = entity.Discount1,
                Discount2 = entity.Discount2,
                Cost = entity.Cost,
                Cost1 = entity.Cost1,
                Cost2 = entity.Cost2
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertDeliveryChallanDetail()
        {
            try
            {
                _db.Task_DeliveryChallanDetail.Add(_entity);
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