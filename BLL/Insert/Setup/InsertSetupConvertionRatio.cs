using Inventory360DataModel;
using Inventory360DataModel.Setup;
using BLL.Common;
using DAL.DataAccess.Insert.Setup;
using DAL.DataAccess.Select.Configuration;
using DAL.DataAccess.Select.Setup;
using DAL.Interface.Insert.Setup;
using DAL.Interface.Select.Configuration;
using DAL.Interface.Select.Setup;
using System;
using System.Linq;
using System.Transactions;

namespace BLL.Insert.Setup
{
    public class InsertSetupConvertionRatio : GenerateDifferentEventPrefix
    {

        public CommonResult InsertConvertionRatio(CommonSetupConvertionRatio entity)
        {
            try
            {
                string generatedNo = string.Empty;

                ISelectSetupConvertionRatio iSelectSetupConvertionRatio = new DSelectSetupConvertionRatio(entity.CompanyId);
                // Get all Convertion Ratio
                var convertionRatioLists = iSelectSetupConvertionRatio.SelectConvertionRatioAll();

                // Check ConvertionRatio for duplicacy
                if (convertionRatioLists.Where(x => x.RatioNo.ToLower() == entity.RatioNo.ToLower()
                    && x.RatioTitle.ToLower() == entity.RatioTitle.ToLower()).Count() > 0)
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Duplicate Convertion Ratio."
                    };
                }

                Guid convertionRatioId = Guid.NewGuid();
                entity.ConvertionRatioId = convertionRatioId;

                // select last finalize no from requisiton finalize nos table   
                string prefix = entity.RatioDate.Year.ToString();
                string previousRatioNo = iSelectSetupConvertionRatio.SelectConvertionRatioWithoutCheckingCompany()
                    .Where(x => x.RatioNo.ToLower().StartsWith(prefix.ToLower()))
                    .OrderByDescending(o => o.RatioNo)
                    .Select(s => s.RatioNo)
                    .FirstOrDefault();
              
                // if no record found, then start with 1
                // otherwise start with next value
                if (string.IsNullOrEmpty(previousRatioNo))
                {
                    generatedNo = prefix + "-" + ("1".PadLeft(6, '0'));
                }
                else
                {
                    long currentValue = 0;
                    long.TryParse(previousRatioNo.Substring(previousRatioNo.Length - 6), out currentValue);
                    long nextValue = ++currentValue;
                    generatedNo = prefix + "-" + (nextValue.ToString().PadLeft(6, '0'));
                }
                entity.RatioNo = generatedNo;


                // Initialize value
                IInsertSetupConvertionRatio iInsertSetupConvertionRatio = new DInsertSetupConvertionRatio(entity);
                iInsertSetupConvertionRatio.InsertConvertionRatio();

                // Convertion Ratio Detail
                foreach (CommonSetupConvertionRatioDetail item in entity.CommonSetupConvertionRatioDetail)
                {
                    item.ConvertionRatioDetailId = Guid.NewGuid();
                    item.ConvertionRatioId = entity.ConvertionRatioId;

                    IInsertSetupConvertionRatioDetail iInsertSetupConvertionRatioDetail = new DInsertSetupConvertionRatioDetail(item);
                    iInsertSetupConvertionRatioDetail.InsertConvertionRatioDetail();                    
                }                
                return new CommonResult()
                {
                    IsSuccess = true,
                    Message = generatedNo,
                    Message1 = convertionRatioId.ToString()
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}