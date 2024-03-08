using System.Collections.Generic;

namespace DAL.Interface.Delete.Task
{
    public interface IDeleteTaskDeliveryChallanDetailSerial
    {
        bool DeleteDeliveryChallanDetailSerial(long productId, long? dimensionId, long unitTypeId, long companyId, List<string> serialLists);
    }
}