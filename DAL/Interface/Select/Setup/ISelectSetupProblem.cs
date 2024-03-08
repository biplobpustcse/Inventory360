using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupProblem
    {
        IQueryable<Setup_Problem> SelectProblemAll();
        IQueryable<Setup_Problem> SelectProblemWithoutCheckingCompany();
    }
}