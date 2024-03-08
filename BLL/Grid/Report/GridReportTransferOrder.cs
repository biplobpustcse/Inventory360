using Inventory360DataModel;
using DAL.DataAccess.Select.Task;
using DAL.Interface.Select.Task;
using System;
using System.Linq;

namespace BLL.Grid.Report
{
    public class GridReportTransferOrder
    {
        public object GenerateTransferOrderReport(long companyId, long locationId, string TransferOrderId, string TransferOrderNo)
        {
            try
            {

                ISelectTaskTransferOrder iSelectTaskTransferOrder = new DSelectTaskTransferOrder(companyId);
                var transferOrderLists = iSelectTaskTransferOrder.SelectTaskTransferOrderAll()
                    .WhereIf(!String.IsNullOrEmpty(TransferOrderId), x => x.OrderId == new Guid(TransferOrderId))
                    .WhereIf(!String.IsNullOrEmpty(TransferOrderNo), x => x.OrderNo == TransferOrderNo)
                    .Where(x => x.LocationId == locationId)
                    .Select(s => new
                    {
                        TransferOrderNo = s.OrderNo,
                        TransferOrderDate = s.OrderDate,
                        TransferOrderBy = s.Setup_Employee.Code + " # " + s.Setup_Employee.ContactNo + " # " + s.Setup_Employee.Name,
                        Approved = s.Approved,
                        ApprovedBy = s.ApprovedBy == null ? "" : s.Security_User.FullName + " [" + s.Security_User.UserName + "]",
                        CancelReason = s.CancelReason,
                        FromLocation = s.Setup_Location1.Name,
                        s.TransferFromStockType,
                        ToLocation = s.Setup_Location.Name,
                        s.TransferToStockType,
                        CompanyName = s.Setup_Company.Name,
                        CompanyAddress = s.Setup_Company.Address,
                        Phone = s.Setup_Company.PhoneNo,
                        Fax = s.Setup_Company.FaxNo == null ? "" : s.Setup_Company.FaxNo,
                        EntryBy = s.Security_User.FullName == null ? "" : s.Security_User.FullName,
                        DetailLists = s.Task_TransferOrderDetail.Select(sd => new
                        {
                            ProductCode = sd.Setup_Product.Code,
                            ProductName = sd.Setup_Product.Name,
                            ProductDimension = sd.ProductDimensionId == null ? "" : ("Measurement : " + sd.Setup_ProductDimension.Setup_Measurement.Name + " # Size : " + sd.Setup_ProductDimension.Setup_Size.Name + " # Style : " + sd.Setup_ProductDimension.Setup_Style.Name + " # Color : " + sd.Setup_ProductDimension.Setup_Color.Name),
                            UnitType = sd.Setup_UnitType.Name,
                            Quantity = sd.Quantity
                        })
                        .OrderBy(o => o.ProductName)
                        .ToList()
                    })
                    .FirstOrDefault();

                if (transferOrderLists != null)
                {
                    return transferOrderLists;
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