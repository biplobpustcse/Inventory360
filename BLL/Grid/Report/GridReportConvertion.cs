using Inventory360DataModel;
using DAL.DataAccess.Select.Task;
using DAL.Interface.Select.Task;
using System;
using System.Linq;

namespace BLL.Grid.Report
{
    public class GridReportConvertion
    {
        public object GenerateConvertionReport(long companyId, long locationId, string ConvertionId, string ConvertionNo)
        {
            try
            {
                ISelectTaskConvertion iSelectTaskConvertion = new DSelectTaskConvertion(companyId);
                var complainConvertionLists = iSelectTaskConvertion.SelectTaskConvertionAll()
                    .WhereIf(!String.IsNullOrEmpty(ConvertionId), x => x.ConvertionId == new Guid(ConvertionId))
                    .WhereIf(!String.IsNullOrEmpty(ConvertionNo), x => x.ConvertionNo == ConvertionNo)
                    .Where(x => x.LocationId == locationId)
                    .Select(s => new
                    {
                        s.ConvertionNo,
                        s.ConvertionDate,
                        ConvertionType = s.ConvertionType == "A" ? "Assemble" : "Disassamble",
                        Approved = s.Approved == "A" ? "Approved" : "Unapproved",
                        ApprovedBy = s.ApprovedBy == null ? "" : s.Security_User.FullName + " [" + s.Security_User.UserName + "]",
                        EntryByName = s.Security_User1.FullName == null ? "" : s.Security_User1.FullName,
                        s.Setup_ConvertionRatio.RatioNo,
                        s.Remarks,
                        s.CancelReason,
                        Location = s.Setup_Location.Name,                        
                        CompanyName = s.Setup_Company.Name,
                        CompanyAddress = s.Setup_Company.Address,
                        Phone = s.Setup_Company.PhoneNo,
                        Fax = s.Setup_Company.FaxNo == null ? "" : s.Setup_Company.FaxNo,
                        ConvertionDetail = s.Task_ConvertionDetail.Select(sd => new
                        {
                            sd.ConvertionDetailId,
                            ProductCode = sd.Setup_Product.Code,
                            ProductName = sd.Setup_Product.Name,
                            ProductFor = sd.ProductFor,
                            ProductDimension = sd.ProductDimensionId == null ? "" : ("Measurement : " + sd.Setup_ProductDimension.Setup_Measurement.Name + " # Size : " + sd.Setup_ProductDimension.Setup_Size.Name + " # Style : " + sd.Setup_ProductDimension.Setup_Style.Name + " # Color : " + sd.Setup_ProductDimension.Setup_Color.Name),
                            UnitType = sd.Setup_UnitType.Name,
                            sd.Quantity,
                            ConvertionDetailSerial = sd.Task_ConvertionDetailSerial.Select(sdp => new
                            {
                                ConvertionDetailId = sdp.ConvertionDetailId,
                                Serial = sdp.Serial,
                                AdditionalSerial = sdp.AdditionalSerial
                            }).ToList()                           
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