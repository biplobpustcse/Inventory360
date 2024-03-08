using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupProfession
    {
        IQueryable<Setup_Profession> SelectProfessionAll();
    }
}