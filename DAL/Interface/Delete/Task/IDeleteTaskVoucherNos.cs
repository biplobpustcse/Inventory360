using System;

namespace DAL.Interface.Delete.Task
{
    public interface IDeleteTaskVoucherNos
    {
        bool DeleteVoucherNos(string prefix, long year, long companyId);
    }
}
