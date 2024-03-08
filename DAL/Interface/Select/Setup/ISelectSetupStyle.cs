using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupStyle
    {
        IQueryable<Setup_Style> SelectStyleAll();
    }
}