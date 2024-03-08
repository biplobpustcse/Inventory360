using DAL.DataAccess.Select.Task;
using DAL.Interface.Select.Task;
using System;
using System.Linq;

namespace BLL.Grid.Task
{
    public class GridTaskTranReqFinalizeDetail
    {
        public object SelectTransferRequisitionFinalizeDetail(long companyId, Guid requisitionId)
        {
            try
            {
                ISelectTaskTransferRequisitionFinalize iSelectTaskTransferRequisitionFinalize = new DSelectTaskTransferRequisitionFinalize(companyId);

                var requisitionDetailItems = iSelectTaskTransferRequisitionFinalize.SelectStockTransferRequisitionFinalizeAll()
                    .Where(x => x.RequisitionId == requisitionId)
                    .Select(s => new
                    {
                        s.RequisitionNo,
                        s.RequisitionDate,
                        s.Approved,
                        s.Remarks,
                        DetailLists = s.Task_TransferRequisitionFinalizeDetail.Select(sd => new
                        {
                            ProductCode = sd.Setup_Product.Code,
                            ProductName = sd.Setup_Product.Name,
                            ProductDimension = sd.ProductDimensionId == null ? null : ("Measurement : " + sd.Setup_ProductDimension.Setup_Measurement.Name + " # Size : " + sd.Setup_ProductDimension.Setup_Size.Name + " # Style : " + sd.Setup_ProductDimension.Setup_Style.Name + " # Color : " + sd.Setup_ProductDimension.Setup_Color.Name),
                            UnitType = sd.Setup_UnitType.Name,
                            Quantity = sd.Quantity,
                            ItemRequisitionNo = sd.ItemRequisitionId == null ? string.Empty : sd.Task_ItemRequisition.RequisitionNo
                        })
                        .OrderBy(o => o.ProductName)
                        .ToList()
                    })
                    .FirstOrDefault();

                if (requisitionDetailItems != null)
                {
                    return requisitionDetailItems;
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