using Inventory360Entity;
using System;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskCustomerDeliveryDetail
    {
        IQueryable<Task_CustomerDeliveryDetail> SelectCustomerDeliveryDetailAll();
    }
}