using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Configuration
{
    public interface ISelectConfigurationOperationType
    {
        IQueryable<Configuration_OperationType> SelectOperationTypeAll();
    }
}