using System;

namespace DAL.Interface.Update.Task
{
    public interface IUpdateTaskChequeInfo
    {
        bool UpdateChequeInfo(string status, DateTime statusDate);
    }
}