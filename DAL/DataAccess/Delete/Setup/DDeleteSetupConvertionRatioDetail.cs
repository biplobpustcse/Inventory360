using Inventory360Entity;
using DAL.Interface.Delete.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Delete.Setup
{
    public class DDeleteSetupConvertionRatioDetail : IDeleteSetupConvertionRatioDetail
    {
        private Inventory360Entities _db;

        public DDeleteSetupConvertionRatioDetail()
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool DeleteConvertionRatioDetail(List<Guid> convertionRatioDetailIds)
        {
            try
            {
                _db.Setup_ConvertionRatioDetail
                    .RemoveRange(
                        _db.Setup_ConvertionRatioDetail
                        .Where(x => convertionRatioDetailIds.Contains(x.ConvertionRatioDetailId)
                        )
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