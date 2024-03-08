using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupTemplateHeader
    {
        IQueryable<Setup_TemplateHeader> SelectTemplateHeaderAll();
    }
}