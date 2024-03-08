using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskItemRequisition : IInsertTaskItemRequisition
    {
        private Inventory360Entities _db;
        private Task_ItemRequisition _entity;

        public DInsertTaskItemRequisition(CommonTaskItemRequisition entity)
        {
            _db = new Inventory360Entities();
            _entity = new Task_ItemRequisition
            {
                RequisitionId = entity.RequisitionId,
                RequisitionNo = entity.RequisitionNo,
                RequisitionDate = entity.RequisitionDate + DateTime.Now.TimeOfDay,
                RequestedBy = entity.RequestedBy,
                Remarks = entity.Remarks,
                Approved = "N",
                LocationId = entity.LocationId,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertItemRequisition()
        {
            try
            {
                _db.Task_ItemRequisition.Add(_entity);
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