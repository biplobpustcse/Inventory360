using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupRelatedProduct
    {
        IQueryable<Setup_RelatedProduct> SelectRelatedProductAll();
    }
}