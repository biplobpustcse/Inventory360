using Inventory360Entity;
using DAL.Interface.Delete.Task;
using System;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Delete.Task
{
    public class DDeleteTaskConvertionNos : IDeleteTaskConvertionNos
    {
        private Inventory360Entities _db;

        public DDeleteTaskConvertionNos()
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool DeleteConvertionNos(string prefix, long year, long companyId)
        {
            try
            {
                _db.Task_ConvertionNos
                    .RemoveRange(
                        _db.Task_ConvertionNos
                        .Where(x => x.ConvertionNo.ToLower().StartsWith(prefix.ToLower())
                        && x.Year == year
                        && x.CompanyId == companyId)
                    );
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