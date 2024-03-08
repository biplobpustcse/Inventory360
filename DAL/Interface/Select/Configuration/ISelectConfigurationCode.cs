using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Configuration
{
    public interface ISelectConfigurationCode
    {
        IQueryable<Configuration_Code> SelectCodeAll();
    }
}