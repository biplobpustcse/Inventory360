using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskPostedVoucher
    {
        IQueryable<Task_PostedVoucher> SelectPostedVoucherAll();
    }
}