using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertStockTransferFinalize : IInsertTaskTransferRequisitionFinalize
    {
        private Inventory360Entities _db;
        private Task_TransferRequisitionFinalize _entity;

        public DInsertStockTransferFinalize(CommonTransferRequisitionFinalize entity)
        {
            _db = new Inventory360Entities();
            _entity = new Task_TransferRequisitionFinalize
            {
                RequisitionId = entity.RequisitionId,
                RequisitionNo = entity.RequisitionNo,
                RequisitionDate = entity.RequisitionDate + DateTime.Now.TimeOfDay,
                RequisitionBy = entity.RequisitionBy,
                Remarks = entity.Remarks,
                StockType=entity.StockType,
                ReqToLocationId=entity.TransferToLocationId,
                Approved = "N",
                LocationId = entity.LocationId,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertTransferRequisitionFinalize()
        {
            try
            {
                _db.Task_TransferRequisitionFinalize.Add(_entity);
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