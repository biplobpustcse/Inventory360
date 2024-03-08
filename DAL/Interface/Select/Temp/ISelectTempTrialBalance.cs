using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Temp
{
    public interface ISelectTempTrialBalance
    {
        IQueryable<Temp_TrialBalance> SelectTempTrialBalanceAll();
    }
}