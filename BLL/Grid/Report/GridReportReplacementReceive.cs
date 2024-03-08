using Inventory360DataModel;
using DAL.DataAccess.Select.Task;
using DAL.Interface.Select.Task;
using System;
using System.Linq;

namespace BLL.Grid.Report
{
    public class GridReportReplacementReceive
    {
        public object GenerateReplacementReceive(long companyId, long locationId, string ReceiveId, string ReceiveNo)
        {
            try
            {

                ISelectTaskReplacementReceive iSelectTaskReplacementReceive = new DSelectTaskReplacementReceive(companyId);
                var replacementReceiveLists = iSelectTaskReplacementReceive.SelectTaskReplacementReceiveAll()
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
                        SupplierName = s.Task_ReplacementReceiveDetail.Select(x=>x.Task_ReplacementClaim).FirstOrDefault().Setup_Supplier.Name,
                        SupplierCode = s.Task_ReplacementReceiveDetail.Select(x => x.Task_ReplacementClaim).FirstOrDefault().Setup_Supplier.Code,
                        SupplierAddress = s.Task_ReplacementReceiveDetail.Select(x => x.Task_ReplacementClaim).FirstOrDefault().Setup_Supplier.Address,
                        SupplierPhone = s.Task_ReplacementReceiveDetail.Select(x => x.Task_ReplacementClaim).FirstOrDefault().Setup_Supplier.Phone,
                        s.CancelReason,
                        Location = s.Setup_Location.Name,
                        CompanyName = s.Setup_Company.Name,
                        CompanyAddress = s.Setup_Company.Address,
                        Phone = s.Setup_Company.PhoneNo,
                        Fax = s.Setup_Company.FaxNo == null ? "" : s.Setup_Company.FaxNo,
                        EntryBy = s.Security_User.FullName == null ? "" : s.Security_User.FullName,
                        s.Remarks,
                        s.TotalChargeAmount,
                        s.TotalDiscount,
                        TotalAmount = s.TotalChargeAmount - s.TotalDiscount + s.Task_ReplacementReceive_Charge.Select(x=>x.ChargeAmount).Sum() + s.Task_ReplacementReceiveDetail.Select(x=>x.AdjustmentType == "A" ? (0 + x.AdjustedAmount) : x.AdjustmentType == "D"? (0 - x.AdjustedAmount) : 0).Sum(),
                        ReplacementReceiveDetail = s.Task_ReplacementReceiveDetail.Select(rd => new
                        {
                            rd.ReplacementClaimId,
                            PreviousProductCode = rd.Setup_Product.Code,
                            NewProductCode = rd.Setup_Product1.Code,
                            PreviousProductName = rd.Setup_Product.Name,
                            NewProductName = rd.Setup_Product1.Name,
                            PreviousProductDimension = rd.PreviousProductDimensionId == null ? "" : ("Measurement : " + rd.Setup_ProductDimension.Setup_Measurement.Name + " # Size : " + rd.Setup_ProductDimension.Setup_Size.Name + " # Style : " + rd.Setup_ProductDimension.Setup_Style.Name + " # Color : " + rd.Setup_ProductDimension.Setup_Color.Name),
                            NewProductDimension = rd.NewProductDimensionId == null ? "" : ("Measurement : " + rd.Setup_ProductDimension1.Setup_Measurement.Name + " # Size : " + rd.Setup_ProductDimension1.Setup_Size.Name + " # Style : " + rd.Setup_ProductDimension1.Setup_Style.Name + " # Color : " + rd.Setup_ProductDimension1.Setup_Color.Name),
                            PreviousUnitType = rd.Setup_UnitType.Name,
                            NewUnitType = rd.Setup_UnitType1.Name,
                            rd.PreviousSerial,
                            rd.NewSerial,
                            AdjustmentType = rd.AdjustmentType == "A"? "Addition" : rd.AdjustmentType == "D" ? "Deduction": "",
                            rd.AdjustedAmount,
                            rd.Task_ReplacementClaim.ClaimNo,
                            rd.Task_ReplacementClaim.ClaimDate
                        }).OrderBy(o => o.NewProductName).ToList(),
                        ReplacementReceive_Charge = s.Task_ReplacementReceive_Charge.Select(crc => new
                        {
                            crc.ReceiveId,
                            ChargeName = crc.Configuration_EventWiseCharge.Setup_Charge.Name,
                            crc.ChargeAmount
                        }).ToList()
                    }).FirstOrDefault();

                if (replacementReceiveLists != null)
                {
                    return replacementReceiveLists;
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