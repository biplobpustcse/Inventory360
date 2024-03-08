using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupCountry
    {
        IQueryable<Setup_Country> SelectCountryAll();
    }
}