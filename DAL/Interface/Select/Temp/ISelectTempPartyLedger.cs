using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Temp
{
    public interface ISelectTempPartyLedger
    {
        IQueryable<Temp_PartyLedger> SelectTempPartyLedgerAll();
    }
}