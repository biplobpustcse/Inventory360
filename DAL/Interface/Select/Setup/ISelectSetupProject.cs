using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupProject
    {
        IQueryable<Setup_Project> SelectProjectAll();
    }
}
