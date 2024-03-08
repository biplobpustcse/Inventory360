using Inventory360DataModel;
using DAL.DataAccess.Select.Setup;
using DAL.Interface.Select.Setup;
using System;
using System.Data.Entity;
using System.Linq;

namespace BLL.Grid.Setup
{
    public class GridSetupConvertionRatio
    {
        private CommonRecordInformation<dynamic> SelectConvertionRatio(string query, long companyId, int pageIndex, int pageSize)
        {
            try
            {
                pageSize = pageSize > 100 ? 100 : pageSize;
                int skip = pageSize * (pageIndex - 1);

                ISelectSetupConvertionRatio iSelectSetupConvertionRatio = new DSelectSetupConvertionRatio(companyId);
                var ConvertionRatioLists = iSelectSetupConvertionRatio.SelectConvertionRatioAll()
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.RatioNo.ToLower().Contains(query.ToLower()))
                    .Select(s => new
                    {
                        convertionRatioId = s.ConvertionRatioId,
                        ratioNo = s.RatioNo,
                        ratioDate = s.RatioDate,
                        ratioTitle = s.RatioTitle,
                        description = s.Description,
                        approved = s.Approved,
                        approvedDate = s.ApprovedDate,
                        approvedBy = s.Security_User.UserName,
                        cancelReason = s.CancelReason,
                        commonSetupConvertionRatioDetail = s.Setup_ConvertionRatioDetail.Select(
                            x => new
                            {
                                convertionRatioId = x.ConvertionRatioId,
                                convertionRatioDetailId = x.ConvertionRatioDetailId,
                                productId = x.ProductId,
                                productName = x.Setup_Product.Name,
                                productFor = x.ProductFor,
                                productForDetail = x.ProductFor == "M" ? "Main Product" : x.ProductFor == "C" ? "Component Product" : "",
                                quantity = x.Quantity,
                                remarks = x.Remarks,
                                productDimensionId = x.ProductDimensionId,
                                dimensionName = x.ProductDimensionId == null ? "" : ("Measurement : " + x.Setup_ProductDimension.Setup_Measurement.Name + " # Size : " + x.Setup_ProductDimension.Setup_Size.Name + " # Style : " + x.Setup_ProductDimension.Setup_Style.Name + " # Color : " + x.Setup_ProductDimension.Setup_Color.Name),
                                unitTypeId = x.UnitTypeId,
                                unitTypeName = x.Setup_UnitType.Name
                            })
                    });

                var pagedData = new CommonRecordInformation<dynamic>();
                pagedData.TotalNumberOfRecords = ConvertionRatioLists.Count();
                pagedData.Start = CommonUtility.StartingIndexOfDataGrid((pagedData.TotalNumberOfRecords == 0 ? 0 : pageIndex), pageSize);
                pagedData.End = CommonUtility.EndingIndexOfDataGrid(pagedData.Start, pageSize, pagedData.TotalNumberOfRecords);
                pagedData.LastPageNo = CommonUtility.LastPageNo(pageSize, pagedData.TotalNumberOfRecords);
                pagedData.Data = ConvertionRatioLists
                    .OrderBy(o => new { o.ratioNo })
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();

                return pagedData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object SelectConvertionRatioLists(string query, long companyId, int pageIndex, int pageSize)
        {
            try
            {
                return SelectConvertionRatio(query, companyId, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object SelectConvertionRatioByCompanyId(long companyId)
        {
            try
            {
                ISelectSetupConvertionRatio iSelectSetupConvertionRatio = new DSelectSetupConvertionRatio(companyId);

                return iSelectSetupConvertionRatio.SelectConvertionRatioAll()
                    .Where(x => x.CompanyId == companyId)
                    .OrderBy(o => o.RatioNo)
                    .Take(10)
                    .Select(s => new
                    {
                        convertionRatioId = s.ConvertionRatioId,
                        ratioNo = s.RatioNo,
                        ratioDate = s.RatioDate,
                        ratioTitle = s.RatioTitle,
                        description = s.Description,
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object SelectConvertionRatioById(Guid convertionRatioId, long companyId)
        {
            try
            {
                ISelectSetupConvertionRatio iSelectSetupConvertionRatio = new DSelectSetupConvertionRatio(companyId);
                var convertionRatioLists = iSelectSetupConvertionRatio.SelectConvertionRatioWithoutCheckingCompany()
                    .Where(x => x.ConvertionRatioId == convertionRatioId)
                    .Select(s => new
                    {
                        convertionRatioId = s.ConvertionRatioId,
                        ratioNo = s.RatioNo,
                        ratioDate = s.RatioDate,
                        ratioTitle = s.RatioTitle,
                        description = s.Description,
                        approved = s.Approved,
                        approvedDate = s.ApprovedDate,
                        approvedBy = s.Security_User.UserName,
                        cancelReason = s.CancelReason,
                        commonSetupConvertionRatioDetail = s.Setup_ConvertionRatioDetail.Select(
                            x => new
                            {
                                convertionRatioId = x.ConvertionRatioId,
                                convertionRatioDetailId = x.ConvertionRatioDetailId,
                                productId = x.ProductId,
                                productName = x.Setup_Product.Name,
                                serialAvailable = x.Setup_Product.SerialAvailable,
                                productFor = x.ProductFor,
                                productForDetail = x.ProductFor == "M" ? "Main Product" : x.ProductFor == "C" ? "Component Product" : "",
                                quantity = x.Quantity,
                                remarks = x.Remarks,
                                productDimensionId = x.ProductDimensionId,
                                dimensionName = x.ProductDimensionId == null ? "" : ("Measurement : " + x.Setup_ProductDimension.Setup_Measurement.Name + " # Size : " + x.Setup_ProductDimension.Setup_Size.Name + " # Style : " + x.Setup_ProductDimension.Setup_Style.Name + " # Color : " + x.Setup_ProductDimension.Setup_Color.Name),
                                unitTypeId = x.UnitTypeId,
                                unitTypeName = x.Setup_UnitType.Name
                            })
                    });
                //var pagedData = new CommonRecordInformation<dynamic>();
                //pagedData.Data = convertionRatioLists.OrderBy(a => a.ratioNo).ToList();
                return convertionRatioLists.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}