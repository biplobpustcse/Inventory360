using Inventory360DataModel;
using DAL.DataAccess.Select.Task;
using DAL.Interface.Select.Task;
using System;
using System.Linq;

namespace BLL.Grid.Report
{
    public class GridReportComplainReceive
    {
        public object GenerateComplainReceiveReport(long companyId, long locationId, string ReceiveId, string ReceiveNo)
        {
            try
            {

                ISelectTaskComplainReceive iSelectTaskComplainReceive = new DSelectTaskComplainReceive(companyId);
                var complainReceiveLists = iSelectTaskComplainReceive.SelectComplainReceiveAll()
                    .WhereIf(!String.IsNullOrEmpty(ReceiveId), x => x.ReceiveId == new Guid(ReceiveId))
                    .WhereIf(!String.IsNullOrEmpty(ReceiveNo), x => x.ReceiveNo == ReceiveNo)
                    .Where(x => x.LocationId == locationId)
                    .Select(s => new
                    {
                        s.ReceiveNo,
                        s.ReceiveDate,
                        RequestedBy = s.Setup_Employee.Code + " # " + s.Setup_Employee.ContactNo + " # " + s.Setup_Employee.Name,
                        Approved = s.Approved == "A" ? "Approved" : "Unapproved",
                        ApprovedBy = s.ApprovedBy == null ? "" : s.Security_User.FullName + " [" + s.Security_User.UserName + "]",
                        s.CancelReason,
                        Location = s.Setup_Location.Name,
                        TransferFromStockType = "",
                        ToLocation = s.Setup_Location.Name,
                        TransferToStockType = "",
                        CompanyName = s.Setup_Company.Name,
                        CompanyAddress = s.Setup_Company.Address,
                        Phone = s.Setup_Company.PhoneNo,
                        Fax = s.Setup_Company.FaxNo == null ? "" : s.Setup_Company.FaxNo,
                        EntryBy = s.Security_User.FullName == null ? "" : s.Security_User.FullName,
                        s.Remarks,
                        CustomerName = s.Setup_Customer.Name,
                        CustomerCode = s.Setup_Customer.Code,
                        CustomerAddress = s.Setup_Customer.Address,
                        CustomerPhone = s.Setup_Customer.PhoneNo,
                        s.TotalChargeAmount,
                        
                        ComplainReceiveDetail = s.Task_ComplainReceiveDetail.Select(sd => new
                        {
                            sd.ReceiveDetailId,
                            ProductCode = sd.Setup_Product.Code,
                            ProductName = sd.Setup_Product.Name,
                            ProductDimension = sd.ProductDimensionId == null ? "" : ("Measurement : " + sd.Setup_ProductDimension.Setup_Measurement.Name + " # Size : " + sd.Setup_ProductDimension.Setup_Size.Name + " # Style : " + sd.Setup_ProductDimension.Setup_Style.Name + " # Color : " + sd.Setup_ProductDimension.Setup_Color.Name),
                            UnitType = sd.Setup_UnitType.Name,
                            sd.TotalSpareAmount,
                            sd.Serial,
                            sd.Remarks,
                            ComplainReceiveDetail_Problem = sd.Task_ComplainReceiveDetail_Problem.Select(sdp => new
                            {
                                ReceiveDetailId = sdp.ReceiveDetailId,
                                ProblemName = sdp.Setup_Problem.Name,
                                ProblemNote = sdp.Note
                            }).ToList()
                            ,
                            ComplainReceiveDetail_SpareProduct = sd.Task_ComplainReceiveDetail_SpareProduct.Select(sdsp => new
                            {
                                ReceiveDetailId = sdsp.ReceiveDetailId,
                                ProductName = sdsp.Setup_Product.Name,
                                sdsp.Quantity,
                                sdsp.Price
                            }).ToList()
                        }).OrderBy(o => o.ProductName).ToList(),
                        ComplainReceive_Charge = s.Task_ComplainReceive_Charge.Select(crc=>new
                        {
                            crc.ReceiveId,
                            ChargeName = crc.Configuration_EventWiseCharge.Setup_Charge.Name,
                            crc.ChargeAmount
                        }).ToList()
                    }).FirstOrDefault();

                if (complainReceiveLists != null)
                {
                    return complainReceiveLists;
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