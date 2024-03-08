using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupAccountsSubGroup
    {
        IQueryable<Setup_AccountsSubGroup> SelectAccountsSubGroupAll();
    }
}
