using Inventory360Entity;
using System;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskReceiveFinalize
    {
        IQueryable<Task_ReceiveFinalize> SelectReceiveFinalizeAll();
        bool CheckReceiveFinalizeIsSettledByPayment(Guid id);
    }
}