using System;

namespace DAL.Interface.Delete.Task
{
    public interface IDeleteTaskSalesOrderDeliveryInfo
    {
        bool DeleteSalesOrderDeliveryInfo(Guid salesOrderId, long companyId);
    }
}