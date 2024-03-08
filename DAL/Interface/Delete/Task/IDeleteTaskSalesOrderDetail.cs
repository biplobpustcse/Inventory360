using System;

namespace DAL.Interface.Delete.Task
{
    public interface IDeleteTaskSalesOrderDetail
    {
        bool DeleteSalesOrderDetail(Guid salesOrderId, long companyId);
    }
}