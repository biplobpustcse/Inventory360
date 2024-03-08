using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskRequisitionFinalizeNos : IInsertTaskRequisitionFinalizeNos
    {
        private Inventory360Entities _db;
        private Task_RequisitionFinalizeNos _entity;

        public DInsertTaskRequisitionFinalizeNos(string finalizeNo, long year, long companyId)
        {
            _db = new Inventory360Entities();
            _entity = new Task_RequisitionFinalizeNos
            {
                Id = Guid.NewGuid(),
                RequisitionNo = finalizeNo,
                Year = year,
                CompanyId = companyId
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertRequisitionFinalizeNos()
        {
            try
            {
                _db.Task_RequisitionFinalizeNos.Add(_entity);
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