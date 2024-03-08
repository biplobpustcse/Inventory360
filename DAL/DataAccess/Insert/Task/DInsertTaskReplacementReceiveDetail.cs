using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskReplacementReceiveDetail : IInsertTaskReplacementReceiveDetail
    {
        private Inventory360Entities _db;
        private Task_ReplacementReceiveDetail _entity;

        public DInsertTaskReplacementReceiveDetail(CommonTaskReplacementReceiveDetail entity)
        {
            _db = new Inventory360Entities();
            _entity = new Task_ReplacementReceiveDetail
            {
                ReceiveDetailId = entity.ReceiveDetailId,
                ReceiveId = entity.ReceiveId,
                ReplacementClaimId = entity.ReplacementClaimId,
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
                AdjustmentType = entity.AdjustmentType,
                AdjustedAmount = entity.AdjustedAmount,
                AdjustedAmount1 = entity.AdjustedAmount1,
                AdjustedAmount2 = entity.AdjustedAmount2
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertReplacementReceiveDetail()
        {
            try
            {
                _db.Task_ReplacementReceiveDetail.Add(_entity);
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