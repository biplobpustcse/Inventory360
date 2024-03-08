using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Configuration
{
    public interface ISelectConfigurationCurrencyRate
    {
        IQueryable<Configuration_CurrencyRate> SelectCurrencyRateAll();
    }
}