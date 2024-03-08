using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Others
{
    public interface ISelectOthersReport
    {
        IQueryable<Others_Report> SelectReportNameAll();
    }
}