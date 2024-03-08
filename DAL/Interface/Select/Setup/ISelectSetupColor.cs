using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupColor
    {
        IQueryable<Setup_Color> SelectColorAll();
    }
}
