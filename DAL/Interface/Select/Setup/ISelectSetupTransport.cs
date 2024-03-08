using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupTransport
    {
        IQueryable<Setup_Transport> SelectTransportAll();
    }
}