using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskConvertionNos : IInsertTaskConvertionNos
    {
        private Inventory360Entities _db;
        private Task_ConvertionNos _entity;

        public DInsertTaskConvertionNos(string ConvertionNo, long year, long companyId)
        {
            _db = new Inventory360Entities();
            _entity = new Task_ConvertionNos
            {
                Id = Guid.NewGuid(),
                ConvertionNo = ConvertionNo,
                Year = year,
                CompanyId = companyId
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertConvertionNos()
        {
            try
            {
                _db.Task_ConvertionNos.Add(_entity);
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