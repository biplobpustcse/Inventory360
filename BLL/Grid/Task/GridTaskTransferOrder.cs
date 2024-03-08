using Inventory360DataModel;
using DAL.DataAccess.Select.Setup;
using DAL.DataAccess.Select.Task;
using DAL.Interface.Select.Setup;
using DAL.Interface.Select.Task;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace BLL.Grid.Task
{
    public class GridTaskTransferOrder
    {
        private CommonRecordInformation<dynamic> SelectTransferOrder(string query, string orderStatus, long locationId, long companyId, int pageIndex, int pageSize)
        {
            try
            {
                pageSize = pageSize > 100 ? 100 : pageSize;
                int skip = pageSize * (pageIndex - 1);

                ISelectTaskTransferOrder iSelectTaskTransferOrder = new DSelectTaskTransferOrder(companyId);
                var transferOrderLists = iSelectTaskTransferOrder.SelectTaskTransferOrderAll()
                    .Where(x => x.LocationId == locationId)
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.OrderNo.ToLower().Contains(query.ToLower()))
                    .WhereIf(!string.IsNullOrEmpty(orderStatus), x => x.Approved.Equals(orderStatus))
                    .Select(s => new
                    {
                        TransferOrderId = s.OrderId,
                        TransferOrderNo = s.OrderNo,
                        TransferOrderDate = s.OrderDate,
                        TransferOrderBy = s.Setup_Employee.Code + " # " + s.Setup_Employee.ContactNo + " # " + s.Setup_Employee.Name,
                        TransferFrom = s.Setup_Location1.Name,
                        TransferTo = s.Setup_Location.Name,
                        Approved = s.Approved
                    });

                var pagedData = new CommonRecordInformation<dynamic>();
                pagedData.TotalNumberOfRecords = transferOrderLists.Count();
                pagedData.Start = CommonUtility.StartingIndexOfDataGrid((pagedData.TotalNumberOfRecords == 0 ? 0 : pageIndex), pageSize);
                pagedData.End = CommonUtility.EndingIndexOfDataGrid(pagedData.Start, pageSize, pagedData.TotalNumberOfRecords);
                pagedData.LastPageNo = CommonUtility.LastPageNo(pageSize, pagedData.TotalNumberOfRecords);
                pagedData.Data = transferOrderLists
                    .OrderByDescending(o => o.TransferOrderDate)
                    .ThenByDescending(t => t.TransferOrderNo)
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
        private CommonRecordInformation<dynamic> SelectAllTransferOrderWithDetailLists(string query, string finalizeStatus, long locationId, long companyId)
        {
            try
            {

                ISelectTaskTransferOrder iSelectTaskTransferOrder = new DSelectTaskTransferOrder(companyId);
                var transferOrderLists = iSelectTaskTransferOrder.SelectTaskTransferOrderAll()
                    .Where(x => x.LocationId == locationId)
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.OrderNo.ToLower().Contains(query.ToLower()))
                    .WhereIf(!string.IsNullOrEmpty(finalizeStatus), x => x.Approved.Equals(finalizeStatus))
                    .Select(s => new
                    {
                        isSelected = false,
                        s.OrderId,
                        s.OrderNo,
                        s.OrderDate,
                        RequisitionBy = s.Setup_Employee.Code + " # " + s.Setup_Employee.ContactNo + " # " + s.Setup_Employee.Name,
                        RequisitionTo = s.Setup_Location.Name,
                        Approved = s.Approved,
                        DetailLists = s.Task_TransferOrderDetail.Select(sd => new
                        {
                            ProductCode = sd.Setup_Product.Code,
                            ProductName = sd.Setup_Product.Name,
                            ProductId = sd.ProductId,
                            ProductDimensionId = sd.ProductDimensionId == null ? 0 : sd.ProductDimensionId,
                            ProductDimension = sd.ProductDimensionId == null ? null : ("Measurement : " + sd.Setup_ProductDimension.Setup_Measurement.Name + " # Size : " + sd.Setup_ProductDimension.Setup_Size.Name + " # Style : " + sd.Setup_ProductDimension.Setup_Style.Name + " # Color : " + sd.Setup_ProductDimension.Setup_Color.Name),
                            UnitType = sd.Setup_UnitType.Name,
                            UnitTypeId = sd.Setup_UnitType.UnitTypeId,
                            Quantity = sd.Quantity
                        })
                    });

                var pagedData = new CommonRecordInformation<dynamic>();
                pagedData.TotalNumberOfRecords = transferOrderLists.Count();
                pagedData.Data = transferOrderLists
                    .OrderByDescending(o => o.OrderDate)
                    .ThenByDescending(t => t.OrderNo)
                    .ToList();

                return pagedData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public object SelectUnApprovedTransferOrderLists(string query, long locationId, long companyId, int pageIndex, int pageSize)
        {
            try
            {
                return SelectTransferOrder(query, "N", locationId, companyId, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object SelectApprovedTransferOrderLists(string query, long locationId, long companyId, int pageIndex, int pageSize)
        {
            try
            {
                return SelectTransferOrder(query, "A", locationId, companyId, pageIndex, pageSize);// "" for all
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object SelectAllTransferOrder(string query, long locationId, long companyId, int pageIndex, int pageSize)
        {
            try
            {
                return SelectTransferOrder(query, "", locationId, companyId, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object SelectAllTransferOrderWithDetailLists(string query, long locationId, long companyId)
        {
            try
            {
                return SelectAllTransferOrderWithDetailLists(query, "", locationId, companyId);// "" for all
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object SelectApprovedTransferOrderLists(long companyId, long fromLocation, long toLocation, string fromDate, string toDate, string fromStockType, string toStockType)
        {
            try
            {
                return SelectApprovedTransferOrderListsForTransferChallan(companyId, fromLocation, toLocation, fromDate, toDate, fromStockType, toStockType);// "" for all
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private object SelectApprovedTransferOrderListsForTransferChallan(long companyId, long fromLocation, long toLocation, string fromDate, string toDate, string fromStockType, string toStockType)
        {
            try
            {
                DateTime fromDateConverted = Convert.ToDateTime(fromDate);
                DateTime toDateConverted = Convert.ToDateTime(toDate);

                ISelectTaskTransferOrder iSelectTaskTransferOrder = new DSelectTaskTransferOrder(companyId);
                var transferOrderLists = iSelectTaskTransferOrder.SelectTaskTransferOrderAll()
                    .Where(x => x.TransferFromId == fromLocation)
                    .Where(x => x.TransferToId == toLocation)
                    .WhereIf(!string.IsNullOrWhiteSpace(fromDate), x => x.OrderDate> fromDateConverted)
                    .WhereIf(!string.IsNullOrWhiteSpace(toDate), x => x.OrderDate <= toDateConverted)
                    .WhereIf(!string.IsNullOrWhiteSpace(fromStockType), x => x.TransferFromStockType == fromStockType)
                    .WhereIf(!string.IsNullOrWhiteSpace(toStockType), x => x.TransferFromStockType == toStockType)
                    .Where(x=>x.Approved=="A")
                    .Select(s => new
                    {
                        isSelected = false,
                        s.OrderId,
                        s.OrderNo,
                        s.OrderDate,
                        RequisitionBy = s.Setup_Employee.Code + " # " + s.Setup_Employee.ContactNo + " # " + s.Setup_Employee.Name,
                        RequisitionTo = s.Setup_Location.Name,
                        Approved = s.Approved,
                        DetailLists = s.Task_TransferOrderDetail.Select(sd => new
                        {
                            ProductCode = sd.Setup_Product.Code,
                            ProductName = sd.Setup_Product.Name,
                            ProductId = sd.ProductId,
                            IsSerialAvailable =sd.Setup_Product.SerialAvailable,
                            ProductDimensionId = sd.ProductDimensionId == null ? 0 : sd.ProductDimensionId,
                            ProductDimension = sd.ProductDimensionId == null ? null : ("Measurement : " + sd.Setup_ProductDimension.Setup_Measurement.Name + " # Size : " + sd.Setup_ProductDimension.Setup_Size.Name + " # Style : " + sd.Setup_ProductDimension.Setup_Style.Name + " # Color : " + sd.Setup_ProductDimension.Setup_Color.Name),
                            UnitType = sd.Setup_UnitType.Name,
                            UnitTypeId = sd.Setup_UnitType.UnitTypeId,
                            Quantity = sd.Quantity
                        })
                    });

                var pagedData = new CommonRecordInformation<dynamic>();
                pagedData.TotalNumberOfRecords = transferOrderLists.Count();
                pagedData.Data = transferOrderLists
                    .OrderByDescending(o => o.OrderDate)
                    .ThenByDescending(t => t.OrderNo)
                    .ToList();

                return pagedData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public object SelectProductDetailInfo(long companyId, long locationId, long productid, string orderId)
        {
            ISelectTaskTransferOrder iSelectTaskTransferOrder = new DSelectTaskTransferOrder(companyId);
            try
            {
                var data = iSelectTaskTransferOrder.SelectProductDetailInfo(companyId, locationId, productid, orderId);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object SelectProductWarehouseByLocationForSerialProduct(long companyId, long locationId, long productid, string orderId)
        {
            ISelectSetupProduct iSelectSetupProduct = new DSelectSetupProduct(companyId);
            try
            {
                var data = iSelectSetupProduct.SelectProductWarehouseByLocationForSerialProduct(companyId, locationId, productid, orderId);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object SelectWarehouseBasedSerialNo(long companyId, long locationId, long productid, long warehouseId, string orderId)
        {
            ISelectSetupProduct iSelectSetupProduct = new DSelectSetupProduct(companyId);
            try
            {
                var data = iSelectSetupProduct.SelectWarehouseBasedSerialNo(companyId, locationId, productid, warehouseId, orderId);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object SelectProductWarehouseByLocation(long companyId, long locationId, long productid, string orderId)
        {
            ISelectTaskTransferOrder iSelectTaskTransferOrder = new DSelectTaskTransferOrder(companyId);
            try
            {
              var data= iSelectTaskTransferOrder.SelectProductWarehouseByLocation(companyId, locationId, productid, orderId);
                return data;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public object SelectProductSerialNos(long companyId, long locationId, long productid, string orderId)
        {
            try
            {
                ISelectSetupProduct iSelectSetupProduct = new DSelectSetupProduct(companyId);
                try
                {
                    var data = iSelectSetupProduct.SelectProductAvailableSerial(companyId, locationId, productid, orderId);
                    return data;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}