using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskComplainReceiveDetailProblem
    {
        IQueryable<Task_ComplainReceiveDetail_Problem> SelectComplainReceiveDetailProblemAll();
    }
}