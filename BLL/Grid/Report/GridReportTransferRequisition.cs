using Inventory360DataModel;
using DAL.DataAccess.Select.Task;
using DAL.Interface.Select.Task;
using System;
using System.Linq;

namespace BLL.Grid.Report
{
    public class GridReportTransferRequisition
    {
        public object GenerateTransferRequisitionReport(long companyId, string TransferRequisitionNo, Guid TransferRequisitionId)
        {
            try
            {
                ISelectTaskTransferRequisitionFinalize iSelectTaskTransferRequisitionFinalize = new DSelectTaskTransferRequisitionFinalize(companyId);

                var requisitionNosWithDetail = iSelectTaskTransferRequisitionFinalize.SelectStockTransferRequisitionFinalizeAll()
                    .WhereIf(TransferRequisitionId != Guid.Empty, x => x.RequisitionId == TransferRequisitionId)
                    .WhereIf(!string.IsNullOrEmpty(TransferRequisitionNo), x => x.RequisitionNo == TransferRequisitionNo)
                    .OrderBy(o => o.RequisitionNo)
                    .Select(s => new
                    {
                        s.RequisitionId,
                        s.RequisitionNo,
                        s.RequisitionDate,
                        s.StockType,
                        Approved = s.Approved,
                        ApprovedBy = s.ApprovedBy == null ? "" : s.Security_User.FullName + " [" + s.Security_User.UserName + "]",
                        CancelReason = s.CancelReason,
                        FromLocation = s.Setup_Location.Name,
                        ToLocation = s.Setup_Location1.Name,
                        CompanyName = s.Setup_Company.Name,
                        CompanyAddress = s.Setup_Company.Address,
                        Phone = s.Setup_Company.PhoneNo,
                        Fax = s.Setup_Company.FaxNo,
                        Currency = s.Setup_Company.BaseCurrency,
                        RequestedBy = s.Setup_Employee.Code + " # " + s.Setup_Employee.ContactNo + " # " + s.Setup_Employee.Name,
                        DetailLists = s.Task_TransferRequisitionFinalizeDetail.Select(sd => new
                        {
                            ProductId = sd.ProductId,
                            ProductCode = sd.Setup_Product.Code,
                            ProductName = sd.Setup_Product.Name,
                            ProductDimensionId = sd.ProductDimensionId,
                            ProductDimension = sd.ProductDimensionId == null ? null : ("Measurement : " + sd.Setup_ProductDimension.Setup_Measurement.Name + " # Size : " + sd.Setup_ProductDimension.Setup_Size.Name + " # Style : " + sd.Setup_ProductDimension.Setup_Style.Name + " # Color : " + sd.Setup_ProductDimension.Setup_Color.Name),
                            UnitTypeId = sd.UnitTypeId,
                            UnitType = sd.Setup_UnitType.Name,
                            Quantity = sd.Quantity,
                            FinalizedQuantity = sd.OrderedQuantity
                        })
                        .Where(x => x.Quantity > 0)
                        .OrderBy(o => o.ProductName)
                        .ToList()
                    })
                    .OrderBy(o => o.RequisitionDate)
                    .FirstOrDefault();

                if (requisitionNosWithDetail != null)
                {
                    return requisitionNosWithDetail;
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