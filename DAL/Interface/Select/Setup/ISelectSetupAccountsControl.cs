using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupAccountsControl
    {
        IQueryable<Setup_AccountsControl> SelectAccountsControlAll();
    }
}
