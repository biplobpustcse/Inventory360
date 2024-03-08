using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskPayment
    {
        IQueryable<Task_Payment> SelectPaymentAll();
    }
}