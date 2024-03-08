using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskVoucherDetail
    {
        IQueryable<Task_VoucherDetail> SelectVoucherDetailByVoucherId();
        IQueryable<Task_VoucherDetail> SelectVoucherDetail();
    }
}
