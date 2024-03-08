using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskCustomerDeliveryNos
    {
        IQueryable<Task_CustomerDeliveryNos> SelectCustomerDeliveryNosAll();
    }
}