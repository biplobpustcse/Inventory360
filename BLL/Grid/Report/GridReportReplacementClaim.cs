using Inventory360DataModel;
using DAL.DataAccess.Select.Task;
using DAL.Interface.Select.Task;
using System;
using System.Linq;

namespace BLL.Grid.Report
{
    public class GridReportReplacementClaim
    {
        public object GenerateReplacementClaim(long companyId, long locationId, string ClaimId, string ClaimNo)
        {
            try
            {

                ISelectTaskReplacementClaim iSelectTaskReplacementClaim = new DSelectTaskReplacementClaim(companyId);
                var customerDeliveryLists = iSelectTaskReplacementClaim.SelectTaskReplacementClaimAll()
                    .WhereIf(!String.IsNullOrEmpty(ClaimId), x => x.ClaimId == new Guid(ClaimId))
                    .WhereIf(!String.IsNullOrEmpty(ClaimNo), x => x.ClaimNo == ClaimNo)
                    .Where(x => x.LocationId == locationId)
                    .Select(s => new
                    {
                        s.ClaimNo,
                        s.ClaimDate,
                        RequestedBy = s.Setup_Supplier.Code + " # " + s.Setup_Employee.ContactNo + " # " + s.Setup_Employee.Name,
                        Approved = s.Approved == "A" ? "Approved" : "Unapproved",
                        ApprovedBy = s.ApprovedBy == null ? "" : s.Security_User.FullName + " [" + s.Security_User.UserName + "]",
                        SupplierName = s.Setup_Supplier.Name,
                        SupplierCode = s.Setup_Supplier.Code,
                        SupplierAddress = s.Setup_Supplier.Address,
                        SupplierPhone = s.Setup_Supplier.Phone,
                        s.CancelReason,
                        Location = s.Setup_Location.Name,
                        ToLocation = s.Setup_Location.Name,
                        CompanyName = s.Setup_Company.Name,
                        CompanyAddress = s.Setup_Company.Address,
                        Phone = s.Setup_Company.PhoneNo,
                        Fax = s.Setup_Company.FaxNo == null ? "" : s.Setup_Company.FaxNo,
                        EntryBy = s.Security_User.FullName == null ? "" : s.Security_User.FullName,
                        Remarks = s.Remarks,

                        ReplacementClaimDetail = s.Task_ReplacementClaimDetail.Select(rd => new
                        {
                            rd.ClaimDetailId,
                            ProductCode = rd.Setup_Product.Code,
                            ProductName = rd.Setup_Product.Name,
                            ProductDimension = rd.ProductDimensionId == null ? "" : ("Measurement : " + rd.Setup_ProductDimension.Setup_Measurement.Name + " # Size : " + rd.Setup_ProductDimension.Setup_Size.Name + " # Style : " + rd.Setup_ProductDimension.Setup_Style.Name + " # Color : " + rd.Setup_ProductDimension.Setup_Color.Name),
                            UnitType = rd.Setup_UnitType.Name,
                            rd.Serial,
                            rd.Task_ComplainReceive.ReceiveNo,
                            rd.Task_ComplainReceive.ReceiveDate,
                            ReplacementClaimDetail_Problem = rd.Task_ReplacementClaimDetail_Problem.Select(rcd => new
                            {
                                rcd.ClaimDetailId,
                                ProblemName = rcd.Setup_Problem.Name,
                                ProblemNote = rcd.Note
                            }).ToList()                 
                        }).OrderBy(o => o.ProductName).ToList(),      
                    }).FirstOrDefault();

                if (customerDeliveryLists != null)
                {
                    return customerDeliveryLists;
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