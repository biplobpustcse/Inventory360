using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskReplacementClaimDetail_Problem
    {
        IQueryable<Task_ReplacementClaimDetail_Problem> SelectReplacementClaimDetail_ProblemAll();
    }
}