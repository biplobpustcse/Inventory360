using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupDesignation
    {
        IQueryable<Setup_Designation> SelectDesignationAll();
    }
}
