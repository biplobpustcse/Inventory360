using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskComplainReceiveDetail : IInsertTaskComplainReceiveDetail
    {
        private Inventory360Entities _db;
        private Task_ComplainReceiveDetail _entity;

        public DInsertTaskComplainReceiveDetail(CommonComplainReceiveDetail entity)
        {
            _db = new Inventory360Entities();
            _entity = new Task_ComplainReceiveDetail
            {
                ReceiveDetailId = entity.ReceiveDetailId,
                ReceiveId = entity.ReceiveId,
                InvoiceId = entity.InvoiceId,
                ProductId = entity.ProductId,
                ProductDimensionId = entity.ProductDimensionId == 0 ? null : entity.ProductDimensionId,
                UnitTypeId = entity.UnitTypeId,
                Serial = entity.Serial,
                AdditionalSerial = entity.AdditionalSerial,
                IsWarrantyAvailable = entity.IsWarrantyAvailable,
                IsServiceWarranty = entity.IsServiceWarranty,
                IsOnlyService = entity.IsOnlyService,
                Cost = entity.Cost,
                Cost1 = entity.Cost1,
                Cost2 = entity.Cost2,
                Remarks = entity.Remarks
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertComplainReceiveDetail()
        {
            try
            {
                _db.Task_ComplainReceiveDetail.Add(_entity);
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