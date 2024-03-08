using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskTransferRequisitionFinalizeNos : IInsertTaskTransferRequisitionFinalizeNos
    {
        private Inventory360Entities _db;
        private Task_TransferRequisitionFinalizeNos _entity;

        public DInsertTaskTransferRequisitionFinalizeNos(string finalizeNo, long year, long companyId)
        {
            _db = new Inventory360Entities();
            _entity = new Task_TransferRequisitionFinalizeNos
            {
                Id = Guid.NewGuid(),
                RequisitionNo = finalizeNo,
                Year = year,
                CompanyId = companyId
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertTransferRequisitionFinalizeNos()
        {
            try
            {
                _db.Task_TransferRequisitionFinalizeNos.Add(_entity);
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