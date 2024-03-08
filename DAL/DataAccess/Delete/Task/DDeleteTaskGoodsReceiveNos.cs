using Inventory360Entity;
using DAL.Interface.Delete.Task;
using System;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Delete.Task
{
    public class DDeleteTaskGoodsReceiveNos : IDeleteTaskGoodsReceiveNos
    {
        private Inventory360Entities _db;

        public DDeleteTaskGoodsReceiveNos()
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool DeleteGoodsReceiveNos(string prefix, long year, long companyId)
        {
            try
            {
                _db.Task_GoodsReceiveNos
                    .RemoveRange(
                        _db.Task_GoodsReceiveNos
                        .Where(x => x.ReceiveNo.ToLower().StartsWith(prefix.ToLower())
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