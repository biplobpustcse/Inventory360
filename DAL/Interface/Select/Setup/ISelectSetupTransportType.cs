using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupTransportType
    {
        IQueryable<Setup_TransportType> SelectTransportTypeAll();
    }
}