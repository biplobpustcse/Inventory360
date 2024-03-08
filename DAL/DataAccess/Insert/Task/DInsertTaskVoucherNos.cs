using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskVoucherNos : IInsertTaskVoucherNos
    {
        private Inventory360Entities _db;
        private Task_VoucherNos _entity;

        public DInsertTaskVoucherNos(string voucherNo, long year, long companyId)
        {
            _db = new Inventory360Entities();
            _entity = new Task_VoucherNos
            {
                Id = Guid.NewGuid(),
                VoucherNo = voucherNo,
                Year = year,
                CompanyId = companyId
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertVoucherNos()
        {
            try
            {
                _db.Task_VoucherNos.Add(_entity);
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
