using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Configuration
{
    public interface ISelectConfigurationFormattingTag
    {
        IQueryable<Configuration_FormattingTag> SelectFormattingTagAll();
    }
}