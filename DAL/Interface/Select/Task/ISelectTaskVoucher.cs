using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskVoucher
    {
        IQueryable<Task_Voucher> SelectVoucherAll();
    }
}