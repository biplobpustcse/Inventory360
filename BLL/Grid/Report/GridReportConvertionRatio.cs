using Inventory360DataModel;
using DAL.DataAccess.Select.Setup;
using DAL.Interface.Select.Setup;
using System;
using System.Linq;

namespace BLL.Grid.Report
{
    public class GridReportConvertionRatio
    {
        public object GenerateConvertionRatioReport(long companyId, long locationId, string ConvertionRatioId, string RatioNo)
        {
            try
            {
                ISelectSetupConvertionRatio iSelectTaskConvertion = new DSelectSetupConvertionRatio(companyId);
                var complainConvertionLists = iSelectTaskConvertion.SelectConvertionRatioAll()
                    .WhereIf(!String.IsNullOrEmpty(ConvertionRatioId), x => x.ConvertionRatioId == new Guid(ConvertionRatioId))
                    .WhereIf(!String.IsNullOrEmpty(RatioNo), x => x.RatioNo == RatioNo)                    
                    .Select(s => new
                    {
                        s.RatioNo,
                        s.RatioDate, 
                        Approved = s.Approved == "A" ? "Approved" : "Unapproved",
                        ApprovedBy = s.ApprovedBy == null ? "" : s.Security_User.FullName + " [" + s.Security_User.UserName + "]",
                        EntryByName = s.Security_User.FullName == null ? "" : s.Security_User.FullName,                   
                        s.CancelReason,                                              
                        CompanyName = s.Setup_Company.Name,
                        CompanyAddress = s.Setup_Company.Address,
                        Phone = s.Setup_Company.PhoneNo,
                        Fax = s.Setup_Company.FaxNo == null ? "" : s.Setup_Company.FaxNo,
                        ConvertionRatioDetail = s.Setup_ConvertionRatioDetail.Select(sd => new
                        {
                            sd.ConvertionRatioDetailId,
                            ProductCode = sd.Setup_Product.Code,
                            ProductName = sd.Setup_Product.Name,
                            ProductFor = sd.ProductFor,
                            ProductDimension = sd.ProductDimensionId == null ? "" : ("Measurement : " + sd.Setup_ProductDimension.Setup_Measurement.Name + " # Size : " + sd.Setup_ProductDimension.Setup_Size.Name + " # Style : " + sd.Setup_ProductDimension.Setup_Style.Name + " # Color : " + sd.Setup_ProductDimension.Setup_Color.Name),
                            UnitType = sd.Setup_UnitType.Name,
                            sd.Quantity,                                                     
                        }).OrderBy(o => o.ProductName).ToList(),                      
                    }).FirstOrDefault();

                if (complainConvertionLists != null)
                {
                    return complainConvertionLists;
                }
                else
                {
                    throw new Exception("No record found");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}