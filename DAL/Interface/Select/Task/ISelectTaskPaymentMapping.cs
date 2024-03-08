using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskPaymentMapping
    {
        IQueryable<Task_PaymentMapping> SelectPaymentMappingAll();
    }
}