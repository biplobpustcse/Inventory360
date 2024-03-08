using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupConvertionRatio
    {
        IQueryable<Setup_ConvertionRatio> SelectConvertionRatioAll();
        IQueryable<Setup_ConvertionRatio> SelectConvertionRatioWithoutCheckingCompany();        
    }
}