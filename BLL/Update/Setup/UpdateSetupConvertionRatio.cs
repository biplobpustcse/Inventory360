using Inventory360DataModel;
using Inventory360DataModel.Setup;
using Inventory360Entity;
using BLL.Common;
using DAL.DataAccess.Delete.Setup;
using DAL.DataAccess.Insert.Setup;
using DAL.DataAccess.Select.Configuration;
using DAL.DataAccess.Select.Setup;
using DAL.DataAccess.Update.Setup;
using DAL.Interface.Delete.Setup;
using DAL.Interface.Insert.Setup;
using DAL.Interface.Select.Configuration;
using DAL.Interface.Select.Setup;
using DAL.Interface.Update.Setup;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Update.Setup
{
    public class UpdateSetupConvertionRatio : GenerateDifferentEventPrefix
    {
        public CommonResult UpdateConvertionRatioSetup(CommonSetupConvertionRatio entity)
        {
            try
            {
                ISelectSetupConvertionRatio iSelectSetupConvertionRatio = new DSelectSetupConvertionRatio(entity.CompanyId);
                // Get all problem
                var convertionRatioLists = iSelectSetupConvertionRatio.SelectConvertionRatioAll();

                // Check problem id that is exist or not
                if (convertionRatioLists.Where(x => x.ConvertionRatioId.Equals(entity.ConvertionRatioId)).Count() == 0)
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Can not edit selected Convertion Ratio. Because of invalid Id provided."
                    };
                }

                var listsExcludeSelectedConvertionRatio = convertionRatioLists
                    .Where(x => x.ConvertionRatioId != entity.ConvertionRatioId);

                // Check ConvertionRatio Title for duplicacy
                if (listsExcludeSelectedConvertionRatio.Where(x => x.RatioTitle.ToLower() == entity.RatioTitle
                   ).Count() > 0)
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Duplicate Convertion Ratio Title."
                    };
                }

                //delete previous Convertion Ratio Detail
                ISelectSetupConvertionRatioDetail iSelectSetupConvertionRatioDetail = new DSelectSetupConvertionRatioDetail(entity.CompanyId);
                IQueryable<Setup_ConvertionRatioDetail> convertionRatioDetaildata = iSelectSetupConvertionRatioDetail.SelectConvertionRatioDetailByConvertionRatioId(entity.ConvertionRatioId);

                IDeleteSetupConvertionRatioDetail iDeleteSetupConvertionRatioDetail = new DDeleteSetupConvertionRatioDetail();
                List<Guid> convertionRatioDetailIds = convertionRatioDetaildata.Select(x=>x.ConvertionRatioDetailId).ToList();
                iDeleteSetupConvertionRatioDetail.DeleteConvertionRatioDetail(convertionRatioDetailIds);
                //Insert new Convertion Ratio Detail
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
                    Message = "Update Successful."                    
                };                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}