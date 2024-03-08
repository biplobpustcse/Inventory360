using Inventory360Entity;
using DAL.Interface.Delete.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Delete.Task
{
    public class DDeleteTaskDeliveryChallanDetailSerial : IDeleteTaskDeliveryChallanDetailSerial
    {
        private Inventory360Entities _db;

        public DDeleteTaskDeliveryChallanDetailSerial()
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool DeleteDeliveryChallanDetailSerial(long productId, long? dimensionId, long unitTypeId, long companyId, List<string> serialLists)
        {
            try
            {
                _db.Task_DeliveryChallanDetailSerial
                    .RemoveRange(
                        _db.Task_DeliveryChallanDetailSerial
                        .Where(x => x.Task_DeliveryChallanDetail.ProductId == productId
                            && x.Task_DeliveryChallanDetail.ProductDimensionId == dimensionId
                            && x.Task_DeliveryChallanDetail.UnitTypeId == unitTypeId
                            && x.Task_DeliveryChallanDetail.Task_DeliveryChallan.CompanyId == companyId
                            && serialLists.Contains(x.Serial)
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