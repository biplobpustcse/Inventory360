using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskGoodsReceiveNos : IInsertTaskGoodsReceiveNos
    {
        private Inventory360Entities _db;
        private Task_GoodsReceiveNos _entity;

        public DInsertTaskGoodsReceiveNos(string receiveNo, long year, long companyId)
        {
            _db = new Inventory360Entities();
            _entity = new Task_GoodsReceiveNos
            {
                Id = Guid.NewGuid(),
                ReceiveNo = receiveNo,
                Year = year,
                CompanyId = companyId
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertGoodsReceiveNos()
        {
            try
            {
                _db.Task_GoodsReceiveNos.Add(_entity);
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