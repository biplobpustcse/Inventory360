using Inventory360DataModel;
using DAL.DataAccess.Select.Stock;
using DAL.DataAccess.Select.Task;
using DAL.Interface.Select.Stock;
using DAL.Interface.Select.Task;
using System;
using System.Linq;

namespace BLL.Grid.Task
{
    public class GridTaskItemRequisitionDetail
    {
        public object SelectItemRequisitionDetailByRequisitionId(long companyId, Guid requisitionId)
        {
            try
            {
                ISelectTaskItemRequisition iSelectTaskItemRequisition = new DSelectTaskItemRequisition(companyId);

                var requisitionDetailItems = iSelectTaskItemRequisition.SelectItemRequisitionAll()
                    .Where(x => x.RequisitionId == requisitionId)
                    .Select(s => new
                    {
                        s.RequisitionNo,
                        s.RequisitionDate,
                        s.Approved,
                        s.Remarks,
                        DetailLists = s.Task_ItemRequisitionDetail.Select(sd => new
                        {
                            ProductCode = sd.Setup_Product.Code,
                            ProductName = sd.Setup_Product.Name,
                            ProductDimension = sd.ProductDimensionId == null ? null : ("Measurement : " + sd.Setup_ProductDimension.Setup_Measurement.Name + " # Size : " + sd.Setup_ProductDimension.Setup_Size.Name + " # Style : " + sd.Setup_ProductDimension.Setup_Style.Name + " # Color : " + sd.Setup_ProductDimension.Setup_Color.Name),
                            UnitType = sd.Setup_UnitType.Name,
                            Quantity = sd.Quantity,
                            sd.RequiredDate,
                            sd.Reason
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

        public object SelectItemRequisitionWithDetailForFinalize(string query, long companyId)
        {
            try
            {
                ISelectTaskItemRequisition iSelectTaskPurchaseRequisition = new DSelectTaskItemRequisition(companyId);

                var requisitionNosWithDetail = iSelectTaskPurchaseRequisition.SelectItemRequisitionAll()
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.RequisitionNo.ToLower().Contains(query.ToLower())
                        || x.Setup_Employee.Name.ToLower().Contains(query.ToLower()))
                    .Where(x => x.Approved.Equals("A") && !x.IsSettled)
                    .Select(s => new
                    {
                        isSelected = false,
                        s.RequisitionId,
                        s.RequisitionNo,
                        s.RequisitionDate,
                        RequestedBy = s.Setup_Employee.Code + " # " + s.Setup_Employee.ContactNo + " # " + s.Setup_Employee.Name,
                        DetailLists = s.Task_ItemRequisitionDetail.Select(sd => new
                        {
                            isItemSelected = false,
                            ProductId = sd.ProductId,
                            ProductCode = sd.Setup_Product.Code,
                            ProductName = sd.Setup_Product.Name,
                            ProductDimensionId = sd.ProductDimensionId,
                            ProductDimension = sd.ProductDimensionId == null ? null : ("Measurement : " + sd.Setup_ProductDimension.Setup_Measurement.Name + " # Size : " + sd.Setup_ProductDimension.Setup_Size.Name + " # Style : " + sd.Setup_ProductDimension.Setup_Style.Name + " # Color : " + sd.Setup_ProductDimension.Setup_Color.Name),
                            UnitTypeId = sd.UnitTypeId,
                            UnitType = sd.Setup_UnitType.Name,
                            Quantity = sd.Quantity,
                            FinalizedQuantity = sd.FinalizedQuantity,
                            givenQuantity = sd.Quantity - sd.FinalizedQuantity
                        })
                        .Where(x => x.givenQuantity > 0)
                        .OrderBy(o => o.ProductName)
                        .ToList()
                    })
                    .OrderBy(o => new { o.RequisitionDate, o.RequisitionNo })
                    .ToList();

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


        public object SelectStockTransferDetailByRequisitionId(long companyId, Guid requisitionId)
        {
            try
            {
                ISelectTaskItemRequisition iSelectTaskItemRequisition = new DSelectTaskItemRequisition(companyId);

                var requisitionDetailItems = iSelectTaskItemRequisition.SelectItemRequisitionAll()
                    .Where(x => x.RequisitionId == requisitionId)
                    .Select(s => new
                    {
                        s.RequisitionNo,
                        s.RequisitionDate,
                        s.Approved,
                        s.Remarks,
                        DetailLists = s.Task_ItemRequisitionDetail.Select(sd => new
                        {
                            ProductCode = sd.Setup_Product.Code,
                            ProductName = sd.Setup_Product.Name,
                            ProductDimension = sd.ProductDimensionId == null ? null : ("Measurement : " + sd.Setup_ProductDimension.Setup_Measurement.Name + " # Size : " + sd.Setup_ProductDimension.Setup_Size.Name + " # Style : " + sd.Setup_ProductDimension.Setup_Style.Name + " # Color : " + sd.Setup_ProductDimension.Setup_Color.Name),
                            UnitType = sd.Setup_UnitType.Name,
                            Quantity = sd.Quantity,
                            sd.RequiredDate,
                            sd.Reason
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

        public object SelectUnApprovedStockTransferWithDetailForFinalize(string query, long companyId,int pageIndex, int pageSize)
        {
            try
            {
                ISelectTaskTransferRequisitionFinalize iSelectStockTransferRequisition = new DSelectTaskTransferRequisitionFinalize(companyId);
                
                pageSize = pageSize > 100 ? 100 : pageSize;
                int skip = pageSize * (pageIndex - 1);

                var requisitionNosWithDetail = iSelectStockTransferRequisition.SelectStockTransferRequisitionFinalizeAll()
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.RequisitionNo.ToLower().Contains(query.ToLower())
                        || x.Setup_Employee.Name.ToLower().Contains(query.ToLower()))
                    .Where(x => x.Approved.Equals("N") && !x.IsSettled)
                    .OrderBy(o => o.RequisitionNo)
                    .Select(s => new
                    {
                        isSelected = false,
                        s.RequisitionId,
                        s.RequisitionNo,
                        s.RequisitionDate,
                        FromLocation=s.Setup_Location.Name,
                        ToLocation=s.Setup_Location1.Name,
                        RequestedBy = s.Setup_Employee.Code + " # " + s.Setup_Employee.ContactNo + " # " + s.Setup_Employee.Name,
                        DetailLists = s.Task_TransferRequisitionFinalizeDetail.Select(sd => new
                        {
                            isItemSelected = false,                            
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
                    .ToList();


                var pagedData = new CommonRecordInformation<dynamic>();
                pagedData.TotalNumberOfRecords = requisitionNosWithDetail.Count();
                pagedData.Start = CommonUtility.StartingIndexOfDataGrid((pagedData.TotalNumberOfRecords == 0 ? 0 : pageIndex), pageSize);
                pagedData.End = CommonUtility.EndingIndexOfDataGrid(pagedData.Start, pageSize, pagedData.TotalNumberOfRecords);
                pagedData.LastPageNo = CommonUtility.LastPageNo(pageSize, pagedData.TotalNumberOfRecords);
                pagedData.Data = requisitionNosWithDetail
                    .OrderByDescending(o => o.RequisitionDate)
                    .ThenByDescending(t => t.RequisitionNo)
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();

                return pagedData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}