using System;

namespace DAL.Interface.Delete.Task
{
    public interface IDeleteTaskVoucher
    {
        bool DeleteVoucher(Guid voucherId, long companyId);
    }
}
