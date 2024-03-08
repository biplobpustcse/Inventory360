using Inventory360DataModel;
using DAL.DataAccess.Select.Task;
using DAL.Interface.Select.Task;
using System;
using System.Linq;

namespace BLL.Grid.Report
{
    public class GridReportCustomerDelivery
    {
        public object GenerateCustomerDeliveryReport(long companyId, long locationId, string DeliveryId, string DeliveryNo)
        {
            try
            {

                ISelectTaskCustomerDelivery iSelectTaskCustomerDelivery = new DSelectTaskCustomerDelivery(companyId);
                var customerDeliveryLists = iSelectTaskCustomerDelivery.SelectTaskCustomerDeliveryAll()
                    .WhereIf(!String.IsNullOrEmpty(DeliveryId), x => x.DeliveryId == new Guid(DeliveryId))
                    .WhereIf(!String.IsNullOrEmpty(DeliveryNo), x => x.DeliveryNo == DeliveryNo)
                    .Where(x => x.LocationId == locationId)
                    .Select(s => new
                    {
                        s.DeliveryNo,
                        s.DeliveryDate,
                        RequestedBy = s.Setup_Customer.Code + " # " + s.Setup_Employee.ContactNo + " # " + s.Setup_Employee.Name,
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
                        Remarks = s.Remarks,
                        s.Discount,
                        CustomerName = s.Setup_Customer.Name,
                        CustomerCode = s.Setup_Customer.Code,
                        CustomerAddress = s.Setup_Customer.Address,
                        CustomerPhone = s.Setup_Customer.PhoneNo,
                        s.TotalChargeAmount,
                        //s.TotalAmount,
                        TotalAmount = s.TotalAmount + s.TotalChargeAmount - s.Discount,

                        CustomerDeliveryDetail = s.Task_CustomerDeliveryDetail.Select(sd => new
                        {
                            sd.DeliveryDetailId,
                            ProductCode = sd.Setup_Product.Code,
                            NewProductName = sd.Setup_Product1.Name,
                            NewProductDimension = sd.NewProductDimensionId == null ? "" : ("Measurement : " + sd.Setup_ProductDimension1.Setup_Measurement.Name + " # Size : " + sd.Setup_ProductDimension1.Setup_Size.Name + " # Style : " + sd.Setup_ProductDimension1.Setup_Style.Name + " # Color : " + sd.Setup_ProductDimension1.Setup_Color.Name),
                            UnitType = sd.Setup_UnitType.Name,
                            sd.TotalSpareAmount,
                            sd.TotalServiceAmount,
                            sd.TotalSpareDiscount,
                            sd.TotalServiceDiscount,
                            sd.AdjustedAmount,
                            sd.NewSerial,
                            CustomerDeliveryDetail_Problem = sd.Task_CustomerDeliveryDetail_Problem.Select(sdp => new
                            {
                                sdp.DeliveryDetailId,
                                ProblemName = sdp.Setup_Problem.Name,
                                ProblemNote = sdp.Note
                            }).ToList()
                            ,
                            CustomerDeliveryDetail_SpareProduct = sd.Task_CustomerDeliveryDetail_SpareProduct.Select(sdsp => new
                            {
                                sdsp.DeliveryDetailId,
                                ProductName = sdsp.Setup_Product.Name,
                                sdsp.Setup_Product.ProductType,
                                sdsp.Quantity,
                                sdsp.Price,
                                sdsp.Discount
                            }).ToList()
                        }).OrderBy(o => o.NewProductName).ToList(),
                        CustomerDelivery_Charge = s.Task_CustomerDelivery_Charge.Select(crc=>new
                        {
                            crc.DeliveryId,
                            ChargeName = crc.Configuration_EventWiseCharge.Setup_Charge.Name,
                            crc.ChargeAmount
                        }).ToList()
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