using Inventory360DataModel;
using DAL.DataAccess.Select.Task;
using DAL.Interface.Select.Task;
using System;
using System.Linq;

namespace BLL.Grid.Task
{
    public class GridTaskTranReqFinalize
    {
        private CommonRecordInformation<dynamic> SelectTransferRequisitionFinalizeLists(string query, string finalizeStatus, long locationId, long companyId, int pageIndex, int pageSize)
        {
            try
            {
                pageSize = pageSize > 100 ? 100 : pageSize;
                int skip = pageSize * (pageIndex - 1);

                ISelectTaskTransferRequisitionFinalize iSelectTaskTransferRequisitionFinalize = new DSelectTaskTransferRequisitionFinalize(companyId);
                var collectionLists = iSelectTaskTransferRequisitionFinalize.SelectStockTransferRequisitionFinalizeAll()
                    .Where(x => x.LocationId == locationId)
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.RequisitionNo.ToLower().Contains(query.ToLower()))
                    .WhereIf(!string.IsNullOrEmpty(finalizeStatus), x => x.Approved.Equals(finalizeStatus))
                    .Select(s => new
                    {
                        s.RequisitionId,
                        s.RequisitionNo,
                        s.RequisitionDate,
                        RequisitionBy = s.Setup_Employee.Code + " # " + s.Setup_Employee.ContactNo + " # " + s.Setup_Employee.Name,
                        RequisitionTo = s.Setup_Location.Name,
                        Approved = s.Approved
                    });

                var pagedData = new CommonRecordInformation<dynamic>();
                pagedData.TotalNumberOfRecords = collectionLists.Count();
                pagedData.Start = CommonUtility.StartingIndexOfDataGrid((pagedData.TotalNumberOfRecords == 0 ? 0 : pageIndex), pageSize);
                pagedData.End = CommonUtility.EndingIndexOfDataGrid(pagedData.Start, pageSize, pagedData.TotalNumberOfRecords);
                pagedData.LastPageNo = CommonUtility.LastPageNo(pageSize, pagedData.TotalNumberOfRecords);
                pagedData.Data = collectionLists
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

        public object SelectUnApprovedTransferRequisitionFinalizeLists(string query, long locationId, long companyId, int pageIndex, int pageSize)
        {
            try
            {
                return SelectTransferRequisitionFinalizeLists(query, "N", locationId, companyId, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object SelectTransferRequisitionFinalizeLists(string query, long locationId, long companyId, int pageIndex, int pageSize)
        {
            try
            {
                return SelectTransferRequisitionFinalizeLists(query, "A", locationId, companyId, pageIndex, pageSize);// "" for all
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object SelectTransferRequisitionFinalizeDetailForTransferOrder(string query, long locationId, long companyId)
        {
            try
            {
                ISelectTaskTransferRequisitionFinalize iSelectTaskTransferRequisitionFinalize = new DSelectTaskTransferRequisitionFinalize(companyId);
                var transferOrderLists = iSelectTaskTransferRequisitionFinalize.SelectStockTransferRequisitionFinalizeAll()
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.RequisitionNo.ToLower().Contains(query.ToLower()))
                    .Where(x => x.Approved.Equals("A")
                        && !x.IsSettled
                        && x.LocationId == locationId)
                    .Select(s => new
                    {
                        isSelected = false,
                        s.RequisitionId,
                        s.RequisitionNo,
                        s.RequisitionDate,
                        RequisitionBy = s.Setup_Employee.Code + " # " + s.Setup_Employee.ContactNo + " # " + s.Setup_Employee.Name,
                        RequisitionTo = s.Setup_Location.Name,
                        Approved = s.Approved,
                        DetailLists = s.Task_TransferRequisitionFinalizeDetail.Select(sd => new
                        {
                            ProductCode = sd.Setup_Product.Code,
                            ProductName = sd.Setup_Product.Name,
                            ProductId = sd.ProductId,
                            ProductDimensionId = sd.ProductDimensionId == null ? 0 : sd.ProductDimensionId,
                            ProductDimension = sd.ProductDimensionId == null ? null : ("Measurement : " + sd.Setup_ProductDimension.Setup_Measurement.Name + " # Size : " + sd.Setup_ProductDimension.Setup_Size.Name + " # Style : " + sd.Setup_ProductDimension.Setup_Style.Name + " # Color : " + sd.Setup_ProductDimension.Setup_Color.Name),
                            UnitType = sd.Setup_UnitType.Name,
                            UnitTypeId = sd.Setup_UnitType.UnitTypeId,
                            Quantity = sd.Quantity,
                            OrderQuantity = sd.OrderedQuantity,
                            GivenQuantity = sd.Quantity - sd.OrderedQuantity
                        })
                        .Where(x => x.GivenQuantity > 0)
                        .OrderBy(o => o.ProductName)
                        .ToList()
                    })
                    .OrderBy(o => o.RequisitionDate)
                    .ToList();

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