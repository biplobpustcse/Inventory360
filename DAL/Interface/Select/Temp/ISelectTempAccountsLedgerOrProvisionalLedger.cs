using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Temp
{
    public interface ISelectTempAccountsLedgerOrProvisionalLedger
    {
        IQueryable<Temp_AccountsLedgerOrProvisionalLedger> SelectTempAccountsLedgerOrProvisionalLedgerAll();
    }
}