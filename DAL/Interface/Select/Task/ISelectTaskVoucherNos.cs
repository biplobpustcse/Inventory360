using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskVoucherNos
    {
        IQueryable<Task_VoucherNos> SelectVoucherNosAll();
    }
}
