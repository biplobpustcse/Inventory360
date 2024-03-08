using Inventory360Entity;
using System;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskCustomerDelivery
    {
        IQueryable<Task_CustomerDelivery> SelectTaskCustomerDeliveryAll();
        IQueryable<Task_CustomerDelivery> SelectTaskCustomerDelivery(Guid Id);
        
    }
}