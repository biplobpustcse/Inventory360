using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskTransferOrder : IInsertTaskTransferOrder
    {
        private Inventory360Entities _db;
        private Task_TransferOrder _entity;

        public DInsertTaskTransferOrder(CommonTransferOrder entity)
        {
            _db = new Inventory360Entities();
            _entity = new Task_TransferOrder
            {
                OrderId = entity.OrderId,
                OrderNo = entity.OrderNo,
                OrderDate = entity.OrderDate + DateTime.Now.TimeOfDay,
                OrderBy = entity.OrderBy,
                Remarks = entity.Remarks,
                Approved = "N",
                TransferFromStockType=entity.TransferFromStockType,
                TransferToStockType=entity.TransferToStockType,
                TransferToId=entity.TransferToId,
                TransferFromId=entity.LocationId,
                LocationId = entity.LocationId,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertTaskTransferOrder()
        {
            try
            {
                _db.Task_TransferOrder.Add(_entity);
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