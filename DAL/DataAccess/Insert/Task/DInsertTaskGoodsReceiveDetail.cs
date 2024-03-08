using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskGoodsReceiveDetail : IInsertTaskGoodsReceiveDetail
    {
        private Inventory360Entities _db;
        private Task_GoodsReceiveDetail _entity;

        public DInsertTaskGoodsReceiveDetail(CommonTaskGoodsReceiveDetail entity, decimal price, decimal price1, decimal price2)
        {
            _db = new Inventory360Entities();
            _entity = new Task_GoodsReceiveDetail
            {
                ReceiveDetailId = entity.ReceiveDetailId,
                ReceiveId = entity.ReceiveId,
                ProductId = entity.ProductId,
                ProductDimensionId = entity.ProductDimensionId == 0 ? null : entity.ProductDimensionId,
                UnitTypeId = entity.UnitTypeId,
                WarehouseId = entity.WarehouseId == 0 ? null : entity.WarehouseId,
                Quantity = entity.Quantity,
                Price = price,
                Price1 = price1,
                Price2 = price2
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertGoodsReceiveDetail()
        {
            try
            {
                _db.Task_GoodsReceiveDetail.Add(_entity);
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