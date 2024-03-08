using DAL.DataAccess.Select.Task;
using DAL.Interface.Select.Task;
using System;
using System.Linq;

namespace BLL.Grid.Task
{
    public class GridTaskTransferOrderDetail
    {
        public object SelectTransferOrderDetail(long companyId, Guid transferOrderid)
        {
            try
            {
                ISelectTaskTransferOrder iSelectTaskTransferOrder = new DSelectTaskTransferOrder(companyId);

                var transferDetailItems = iSelectTaskTransferOrder.SelectTaskTransferOrder(transferOrderid)
                    .Where(x => x.OrderId == transferOrderid)
                    .Select(s => new
                    {
                        TransferOrderNo = s.OrderNo,
                        TransferOrderDate = s.OrderDate,
                        s.Approved,
                        s.Remarks,
                        DetailLists = s.Task_TransferOrderDetail.Select(sd => new
                        {
                            ProductCode = sd.Setup_Product.Code,
                            ProductName = sd.Setup_Product.Name,
                            ProductDimension = sd.ProductDimensionId == null ? null : ("Measurement : " + sd.Setup_ProductDimension.Setup_Measurement.Name + " # Size : " + sd.Setup_ProductDimension.Setup_Size.Name + " # Style : " + sd.Setup_ProductDimension.Setup_Style.Name + " # Color : " + sd.Setup_ProductDimension.Setup_Color.Name),
                            UnitType = sd.Setup_UnitType.Name,
                            Quantity = sd.Quantity,
                            TransferOrderNo = s.OrderNo
                        })
                        .OrderBy(o => o.ProductName)
                        .ToList()
                    })
                    .FirstOrDefault();

                if (transferDetailItems != null)
                {
                    return transferDetailItems;
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