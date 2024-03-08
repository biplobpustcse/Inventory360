using Inventory360Entity;
using System;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupConvertionRatioDetail
    {
        IQueryable<Setup_ConvertionRatioDetail> SelectConvertionRatioDetailAll();
        IQueryable<Setup_ConvertionRatioDetail> SelectConvertionRatioDetailById(Guid convertionRatioDetailId);
        IQueryable<Setup_ConvertionRatioDetail> SelectConvertionRatioDetailByConvertionRatioId(Guid convertionRatioId);
        IQueryable<Setup_ConvertionRatioDetail> SelectConvertionRatioDetailWithoutCheckingCompany();        
    }
}