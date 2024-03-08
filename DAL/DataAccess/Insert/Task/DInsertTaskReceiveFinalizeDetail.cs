using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskReceiveFinalizeDetail : IInsertTaskReceiveFinalizeDetail
    {
        private Inventory360Entities _db;
        private Task_ReceiveFinalizeDetail _entity;

        public DInsertTaskReceiveFinalizeDetail(CommonTaskGoodsReceiveFinalizeDetail entity, decimal price, decimal price1, decimal price2)
        {
            _db = new Inventory360Entities();
            _entity = new Task_ReceiveFinalizeDetail
            {
                FinalizeDetailId = entity.FinalizeDetailId,
                FinalizeId = entity.FinalizeId,
                ReceiveId = entity.ReceiveId,
                ProductId = entity.ProductId,
                ProductDimensionId = entity.ProductDimensionId == 0 ? null : entity.ProductDimensionId,
                UnitTypeId = entity.UnitTypeId,
                Quantity = entity.Quantity,
                Price = price,
                Price1 = price1,
                Price2 = price2
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertReceiveFinalizeDetail()
        {
            try
            {
                _db.Task_ReceiveFinalizeDetail.Add(_entity);
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