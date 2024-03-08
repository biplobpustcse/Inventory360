using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskReplacementClaimDetail : IInsertTaskReplacementClaimDetail
    {
        private Inventory360Entities _db;
        private Task_ReplacementClaimDetail _entity;

        public DInsertTaskReplacementClaimDetail(CommonReplacementClaimDetail entity)
        {
            _db = new Inventory360Entities();
            _entity = new Task_ReplacementClaimDetail
            {
                ClaimDetailId = entity.ClaimDetailId,
                ClaimId = entity.ClaimId,
                ComplainReceiveId = entity.ComplainReceiveId,
                ProductId = entity.ProductId,
                ProductDimensionId = entity.ProductDimensionId == 0 ? null : entity.ProductDimensionId,
                UnitTypeId = entity.UnitTypeId,
                Serial = entity.Serial,
                AdditionalSerial = entity.AdditionalSerial,
                ReceivedSerialNo = entity.ReceivedSerialNo,
                ReceivedAdditionalSerial = entity.ReceivedAdditionalSerial,
                StockInBy = entity.StockInBy,
                StockInRefNo = entity.StockInRefNo ,
                StockInRefDate = entity.StockInRefDate,
                LCOrReferenceNo = entity.LCOrReferenceNo,
                LCOrReferenceDate = entity.LCOrReferenceDate
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertReplacementClaimDetail()
        {
            try
            {
                _db.Task_ReplacementClaimDetail.Add(_entity);
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