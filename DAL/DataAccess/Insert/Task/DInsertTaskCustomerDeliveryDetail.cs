using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskCustomerDeliveryDetail : IInsertTaskCustomerDeliveryDetail
    {
        private Inventory360Entities _db;
        private Task_CustomerDeliveryDetail _entity;

        public DInsertTaskCustomerDeliveryDetail(CommonTaskCustomerDeliveryDetail entity)
        {
            _db = new Inventory360Entities();
            _entity = new Task_CustomerDeliveryDetail
            {
                DeliveryDetailId = entity.DeliveryDetailId,
                DeliveryId = entity.DeliveryId,
                ComplainReceiveId = entity.ComplainReceiveId,
                PreviousProductId = entity.PreviousProductId,
                PreviousProductDimensionId = entity.PreviousProductDimensionId == 0 ? null : entity.PreviousProductDimensionId,
                PreviousUnitTypeId = entity.PreviousUnitTypeId,
                PreviousSerial = entity.PreviousSerial,
                NewProductId = entity.NewProductId,
                NewProductDimensionId = entity.NewProductDimensionId == 0 ? null : entity.NewProductDimensionId,
                NewUnitTypeId = entity.NewUnitTypeId,
                NewSerial = entity.NewSerial,
                Cost = entity.Cost,
                Cost1 = entity.Cost1,
                Cost2 = entity.Cost2,
                IsAdjustmentRequired = entity.IsAdjustmentRequired,
                AdjustmentType = entity.AdjustmentType,
                AdjustedAmount = entity.AdjustedAmount,
                AdjustedAmount1 = entity.AdjustedAmount1,
                AdjustedAmount2 = entity.AdjustedAmount2,
                DeliveryType = entity.DeliveryType,
                TotalSpareAmount = entity.TotalSpareAmount,
                TotalSpareAmount1 = entity.TotalSpareAmount1,
                TotalSpareAmount2 = entity.TotalSpareAmount2,
                TotalSpareDiscount = entity.TotalSpareDiscount,
                TotalSpareDiscount1 = entity.TotalSpareDiscount1,
                TotalSpareDiscount2 = entity.TotalSpareDiscount2
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertCustomerDeliveryDetail()
        {
            try
            {
                _db.Task_CustomerDeliveryDetail.Add(_entity);
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