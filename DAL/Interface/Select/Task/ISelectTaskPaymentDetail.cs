using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskPaymentDetail
    {
        IQueryable<Task_PaymentDetail> SelectPaymentDetailAll();
    }
}