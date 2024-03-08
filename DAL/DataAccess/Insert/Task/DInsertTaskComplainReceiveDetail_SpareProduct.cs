using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskComplainReceiveDetail_SpareProduct : IInsertTaskComplainReceiveDetail_SpareProduct
    {
        private Inventory360Entities _db;
        private Task_ComplainReceiveDetail_SpareProduct _entity;

        public DInsertTaskComplainReceiveDetail_SpareProduct(CommonComplainReceiveDetail_SpareProduct entity)
        {
            _db = new Inventory360Entities();
            _entity = new Task_ComplainReceiveDetail_SpareProduct
            {

                ReceiveDetailSpareId = entity.ReceiveDetailSpareId,
                ReceiveDetailId = entity.ReceiveDetailId,
                ProductId = entity.ProductId,
                ProductDimensionId = entity.ProductDimensionId,
                UnitTypeId = entity.UnitTypeId,
                Quantity = entity.Quantity,
                Price = entity.Price,
                Price1 = entity.Price1,
                Price2 = entity.Price2,
                Discount = entity.Discount,
                Discount1 = entity.Discount1,
                Discount2 = entity.Discount2
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertComplainReceiveDetail_SpareProduct()
        {
            try
            {
                _db.Task_ComplainReceiveDetail_SpareProduct.Add(_entity);
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