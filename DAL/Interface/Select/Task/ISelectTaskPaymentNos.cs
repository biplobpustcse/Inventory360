using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskPaymentNos
    {
        IQueryable<Task_PaymentNos> SelectPaymentNosAll();
    }
}