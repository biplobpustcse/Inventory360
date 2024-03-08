using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskTransferRequisitionFinalize : IInsertTaskRequisitionFinalize
    {
        private Inventory360Entities _db;
        private Task_RequisitionFinalize _entity;

        public DInsertTaskTransferRequisitionFinalize(CommonTaskRequisitionFinalize entity)
        {
            _db = new Inventory360Entities();
            _entity = new Task_RequisitionFinalize
            {
                RequisitionId = entity.RequisitionId,
                RequisitionNo = entity.RequisitionNo,
                RequisitionDate = entity.RequisitionDate + DateTime.Now.TimeOfDay,
                RequisitionBy = entity.RequisitionBy,
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
        public bool InsertRequisitionFinalize()
        {
            try
            {
                _db.Task_RequisitionFinalize.Add(_entity);
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