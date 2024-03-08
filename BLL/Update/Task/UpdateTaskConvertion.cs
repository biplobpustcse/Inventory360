using Inventory360DataModel;
using Inventory360DataModel.Task;
using BLL.Common;
using DAL.DataAccess.Delete.Stock;
using DAL.DataAccess.Insert.Stock;
using DAL.DataAccess.Insert.Task;
using DAL.DataAccess.Select.Stock;
using DAL.DataAccess.Select.Task;
using DAL.DataAccess.Update.Setup;
using DAL.DataAccess.Update.Stock;
using DAL.DataAccess.Update.Task;
using DAL.Interface.Delete.Stock;
using DAL.Interface.Insert.Stock;
using DAL.Interface.Insert.Task;
using DAL.Interface.Select.Stock;
using DAL.Interface.Select.Task;
using DAL.Interface.Update.Setup;
using DAL.Interface.Update.Stock;
using DAL.Interface.Update.Task;
using System;
using System.Linq;
using System.Transactions;

namespace BLL.Update.Task
{
    public class UpdateTaskConvertion
    {
        public CommonResult CancelConvertion(Guid id, string reason, long companyId, long userId)
        {
            try
            {
                ISelectTaskConvertion iSelectTaskConvertion = new DSelectTaskConvertion(companyId);
                var selectedConvertion = iSelectTaskConvertion.SelectTaskConvertionAll()
                    .Where(x => x.ConvertionId == id);

                // Check Convertion already exist or not
                if (selectedConvertion.Count() == 0)
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Selected Convertion not found."
                    };
                }

                // Check Convertion already approved or not
                if (selectedConvertion.Where(x => x.Approved.Equals("A")).Count() > 0)
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Selected Convertion already approved."
                    };
                }

                // Check Convertion already cancelled or not
                if (selectedConvertion.Where(x => x.Approved.Equals("C")).Count() > 0)
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Selected Convertion already cancelled."
                    };
                }

                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, ApplicationState.TransactionOptions))
                {
                    IUpdateTaskConvertion iUpdateTaskConvertion = new DUpdateTaskConvertion(id);
                    bool isSuccess = iUpdateTaskConvertion.UpdateConvertionForCancel(reason, userId);
                    if (isSuccess)
                    {
                        transaction.Complete();

                        return new CommonResult()
                        {
                            IsSuccess = isSuccess,
                            Message = "Cancel Successful."
                        };
                    }
                    else
                    {
                        return new CommonResult()
                        {
                            IsSuccess = false,
                            Message = "Cancel Unsuccessful."
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CommonResult ApproveConvertion(Guid id, long companyId, long locationId, long userId)
        {
            try
            {
                ISelectTaskConvertion iSelectTaskConvertion = new DSelectTaskConvertion(companyId);
                var selectedConvertion = iSelectTaskConvertion.SelectTaskConvertionAll()
                    .Where(x => x.ConvertionId == id);

                // Check Convertion exist or not
                if (selectedConvertion.Count() == 0)
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Selected Convertion No not found."
                    };
                }

                // Check Convertion already approved or not
                if (selectedConvertion.Where(x => x.Approved.Equals("A")).Count() > 0)
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Selected Convertion No already approved."
                    };
                }

                // Check Convertion already cancelled or not
                if (selectedConvertion.Where(x => x.Approved.Equals("C")).Count() > 0)
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Selected Convertion No already cancelled."
                    };
                }

                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, ApplicationState.TransactionOptions))
                {
                    var currencyInfo = GetCompanyCurrencyInfo.CompanyCurrencyInfo(companyId);
                    // update Convertion as approved
                    IUpdateTaskConvertion iUpdateTaskConvertion = new DUpdateTaskConvertion(id);
                    bool isSuccess = iUpdateTaskConvertion.UpdateConvertionForApprove(userId);
                    if (isSuccess)
                    {
                        ISelectTaskConvertionDetail iSelectTaskConvertionDetail = new DSelectTaskConvertionDetail(companyId);
                        var detailLists = iSelectTaskConvertionDetail.SelectTaskConvertionDetailAll()
                            .Where(x => x.Task_Convertion.ConvertionId == id)
                            .Select(s => new
                            {
                                s.ConvertionDetailId,
                                s.Task_Convertion.ConvertionFor,
                                s.Task_Convertion.ConvertionNo,
                                s.Task_Convertion.ConvertionDate,
                                s.Task_Convertion.LocationId,
                                s.ProductFor,
                                s.ProductId,
                                ProductName = s.Setup_Product.Name,
                                s.ProductDimensionId,
                                ProductDimension = s.ProductDimensionId == null ? "" : ("Measurement : " + s.Setup_ProductDimension.Setup_Measurement.Name + " # Size : " + s.Setup_ProductDimension.Setup_Size.Name + " # Style : " + s.Setup_ProductDimension.Setup_Style.Name + " # Color : " + s.Setup_ProductDimension.Setup_Color.Name),
                                s.UnitTypeId,
                                s.PrimaryUnitTypeId,
                                s.SecondaryUnitTypeId,
                                s.TertiaryUnitTypeId,
                                s.SecondaryConversionRatio,
                                s.TertiaryConversionRatio,
                                s.WareHouseId,
                                s.Quantity,
                                s.Cost,
                                s.Cost1,
                                s.Cost2,
                                s.ReferenceNo,
                                s.ReferenceDate,
                                s.GoodsReceiveId,
                                s.ImportedStockInId,
                                s.SupplierId,
                                SerialAvailable = s.Task_ConvertionDetailSerial.Count > 0 ? true : false,
                                SerialLists = s.Task_ConvertionDetailSerial
                            }).ToList();
                        var convertionFor = detailLists.Select(x => x.ConvertionFor).FirstOrDefault();
                        if (convertionFor == "A")
                        {
                            detailLists = detailLists.OrderBy(b => b.ProductFor).ToList();
                        }
                        else
                        {
                            detailLists = detailLists.OrderByDescending(b => b.ProductFor).ToList();
                        }
                        //----------------------------------------
                        foreach (var item in detailLists)
                        {
                            //ConvertionFor == "A" for assemble and ProductFor == "C" for Component product 
                            //ConvertionFor == "D" for Dis assemble and ProductFor == "M" for Main product 
                            if ((item.ConvertionFor == "A" && item.ProductFor == "C") || (item.ConvertionFor == "D" && item.ProductFor == "M"))
                            {
                                ISelectStockCurrentStock iSelectStockCurrentStock = new DSelectStockCurrentStock(companyId);
                                // check current stock table first that item found or not
                                var currentStockInfo = iSelectStockCurrentStock.SelectCurrentStockAll()
                                    .Where(x => x.ProductId == item.ProductId
                                        && x.ProductDimensionId == (item.ProductDimensionId == 0 ? null : item.ProductDimensionId)
                                        && x.UnitTypeId == item.PrimaryUnitTypeId
                                        && x.LocationId == locationId
                                        && x.WareHouseId == (item.WareHouseId == 0 ? null : item.WareHouseId)
                                        && x.Quantity > 0)
                                    .OrderBy(o => o.ReferenceDate)
                                    .Select(s => new
                                    {
                                        s.CurrentStockId,
                                        s.Quantity,
                                        s.Cost,
                                        s.Cost1,
                                        s.Cost2,
                                        s.ReferenceNo,
                                        s.ReferenceDate,
                                        s.GoodsReceiveId,
                                        s.ImportedStockInId,
                                        s.SupplierId
                                    })
                                    .ToList();

                                CommonConvertedStock convertedStock = GetStockConversion.ConvertedStockQtyAndCost(item.UnitTypeId, item.PrimaryUnitTypeId, (item.SecondaryUnitTypeId == null ? 0 : (long)item.SecondaryUnitTypeId), (item.TertiaryUnitTypeId == null ? 0 : (long)item.TertiaryUnitTypeId), item.SecondaryConversionRatio, item.TertiaryConversionRatio, item.Quantity, item.Cost);
                                if (currentStockInfo.Select(s => s.Quantity).DefaultIfEmpty(0).Sum() >= convertedStock.PrimaryStockQty)
                                {
                                    IUpdateStockCurrentStock iUpdateStockCurrentStock = new DUpdateStockCurrentStock();

                                    // add serial to challan detail serial 
                                    // and remove from current stock serial
                                    if (item.SerialAvailable)
                                    {
                                        // check serial first to current stock serial table
                                        var itemWiseSerials = item.SerialLists.Select(s => s.Serial).ToList();

                                        ISelectStockCurrentStockSerial iSelectStockCurrentStockSerial = new DSelectStockCurrentStockSerial(companyId);
                                        var serialLists = iSelectStockCurrentStockSerial.SelectCurrentStockSerialAll()
                                            .Where(x => x.Stock_CurrentStock.ProductId == item.ProductId
                                                && x.Stock_CurrentStock.ProductDimensionId == (item.ProductDimensionId == 0 ? null : item.ProductDimensionId)
                                                && x.Stock_CurrentStock.UnitTypeId == item.UnitTypeId
                                                && x.Stock_CurrentStock.LocationId == locationId
                                                && x.Stock_CurrentStock.WareHouseId == (item.WareHouseId == 0 ? null : item.WareHouseId)
                                                && itemWiseSerials.Contains(x.Serial))
                                            .Select(s => new
                                            {
                                                s.CurrentStockSerialId,
                                                s.CurrentStockId,
                                                s.Serial,
                                                s.AdditionalSerial
                                            })
                                            .ToList();

                                        if (serialLists.Count != itemWiseSerials.Count)
                                        {
                                            throw new Exception("Serial missing in current stock for " + item.ProductName);
                                        }

                                        var currentStockIds = serialLists.Select(s => s.CurrentStockId).Distinct().ToList();
                                        foreach (Guid csIds in currentStockIds)
                                        {
                                            decimal qty = serialLists.Where(x => x.CurrentStockId == csIds).Count();
                                            //update reference
                                            var currentInfo = currentStockInfo.Where(x => x.CurrentStockId == csIds).Select(s => new { s.Cost, s.Cost1, s.Cost2, s.ReferenceNo, s.ReferenceDate, s.GoodsReceiveId, s.ImportedStockInId, s.SupplierId }).FirstOrDefault();                                            
                                            IUpdateTaskConvertionDetail iUpdateTaskConvertionDetail = new DUpdateTaskConvertionDetail(item.ConvertionDetailId);
                                            bool isSuccessRef = iUpdateTaskConvertionDetail.UpdateConvertionDetailReference(currentInfo.GoodsReceiveId, currentInfo.ImportedStockInId, currentInfo.SupplierId, currentInfo.ReferenceNo, currentInfo.ReferenceDate);
                                            
                                            var serialListByCurrentStockId = serialLists.Where(x => x.CurrentStockId == csIds).Select(s => new { s.CurrentStockSerialId, s.Serial, s.AdditionalSerial }).ToList();

                                            // remove serial from current stock serial table
                                            IDeleteTaskCurrentStockSerial iDeleteTaskCurrentStockSerial = new DDeleteTaskCurrentStockSerial();
                                            iDeleteTaskCurrentStockSerial.DeleteCurrentStockSerial(serialListByCurrentStockId.Select(s => s.CurrentStockSerialId).ToList());

                                            // deduct qty by current stock ids into current stock table
                                            iUpdateStockCurrentStock.UpdateCurrentStockQuantityByDecrease(csIds, qty);                                            
                                        }
                                    }
                                    else
                                    {
                                        foreach (var csItem in currentStockInfo)
                                        {

                                            decimal deductableQty = (csItem.Quantity >= convertedStock.PrimaryStockQty ? convertedStock.PrimaryStockQty : csItem.Quantity);
                                            convertedStock.PrimaryStockQty -= deductableQty;

                                            iUpdateStockCurrentStock.UpdateCurrentStockQuantityByDecrease(csItem.CurrentStockId, deductableQty);
                                            //update reference
                                            var currentInfo = currentStockInfo.Where(x => x.CurrentStockId == csItem.CurrentStockId).Select(s => new { s.Cost, s.Cost1, s.Cost2, s.ReferenceNo, s.ReferenceDate, s.GoodsReceiveId, s.ImportedStockInId, s.SupplierId }).FirstOrDefault();
                                            IUpdateTaskConvertionDetail iUpdateTaskConvertionDetail = new DUpdateTaskConvertionDetail(item.ConvertionDetailId);
                                            bool isSuccessRef = iUpdateTaskConvertionDetail.UpdateConvertionDetailReference(currentInfo.GoodsReceiveId, currentInfo.ImportedStockInId, currentInfo.SupplierId, currentInfo.ReferenceNo, currentInfo.ReferenceDate);


                                            if (convertedStock.PrimaryStockQty == 0) break;
                                        }
                                    }
                                }
                                else
                                {
                                    throw new Exception(item.ProductName + (string.IsNullOrEmpty(item.ProductDimension) ? string.Empty : " and Dimension : ") + item.ProductDimension + " have not enough stock for convertion.");
                                }
                            }
                            //ConvertionFor == "A" for assemble and ProductFor == "M" for Main product 
                            //ConvertionFor == "D" for Dis assemble and ProductFor == "C" for Component product 
                            else if ((item.ConvertionFor == "A" && item.ProductFor == "M") || (item.ConvertionFor == "D" && item.ProductFor == "C"))
                            {
                                IUpdateStockCurrentStock iUpdateStockCurrentStock = new DUpdateStockCurrentStock();

                                ISelectStockCurrentStock iSelectStockCurrentStock = new DSelectStockCurrentStock(companyId);
                                // check current stock table first that item found or not
                                var currentStockInfo = iSelectStockCurrentStock.SelectCurrentStockAll()
                                    .Where(x => x.ProductId == item.ProductId
                                        && x.ProductDimensionId == (item.ProductDimensionId == 0 ? null : item.ProductDimensionId)
                                        && x.UnitTypeId == item.PrimaryUnitTypeId
                                        && x.LocationId == locationId
                                        && x.WareHouseId == (item.WareHouseId == 0 ? null : item.WareHouseId)
                                        )
                                    .OrderBy(o => o.ReferenceDate)
                                    .Select(s => new
                                    {
                                        s.CurrentStockId,
                                        s.ProductId,
                                        s.UnitTypeId,
                                        s.ProductDimensionId,
                                        s.WareHouseId,
                                        s.Quantity,
                                        s.Cost,
                                        s.Cost1,
                                        s.Cost2,
                                        s.ReferenceNo,
                                        s.ReferenceDate,
                                        s.GoodsReceiveId,
                                        s.ImportedStockInId,
                                        s.SupplierId
                                    })
                                    .ToList();
                                CommonConvertedStock convertedStock = GetStockConversion.ConvertedStockQtyAndCost(item.UnitTypeId, item.PrimaryUnitTypeId, (item.SecondaryUnitTypeId == null ? 0 : (long)item.SecondaryUnitTypeId), (item.TertiaryUnitTypeId == null ? 0 : (long)item.TertiaryUnitTypeId), item.SecondaryConversionRatio, item.TertiaryConversionRatio, item.Quantity, item.Cost);

                                //Guid CurrentStockId = currentStockInfo.Select(s => s.CurrentStockId).LastOrDefault();
                                var CurrentStock = currentStockInfo.Select(s => new { s.CurrentStockId,s.ProductId,s.UnitTypeId, s.ProductDimensionId,s.WareHouseId,s.Cost,s.Cost1,s.Cost2,s.ReferenceNo,s.ReferenceDate }).LastOrDefault();

                                if (CurrentStock != null)
                                {
                                    iUpdateStockCurrentStock.UpdateCurrentStockQuantityByIncrease(CurrentStock.CurrentStockId, item.Quantity);
                                    //update stock reference
                                    iUpdateStockCurrentStock.UpdateCurrentStockReference(CurrentStock.CurrentStockId, item.GoodsReceiveId, item.ImportedStockInId, item.SupplierId, item.ConvertionNo, item.ConvertionDate);                                   
                                    //
                                    if (item.SerialAvailable)
                                    {
                                        foreach (var itemSerial in item.SerialLists.ToList())
                                        {
                                            IInsertStockCurrentStockSerial iInsertStockCurrentStockSerial = new DInsertStockCurrentStockSerial(
                                                CurrentStock.CurrentStockId,
                                                itemSerial.Serial,
                                                itemSerial.AdditionalSerial
                                                );
                                            iInsertStockCurrentStockSerial.InsertCurrentStockSerial();
                                        }
                                    }
                                }
                                else
                                {
                                    Guid stockId = Guid.NewGuid();
                                    // save CurrentStock info
                                    IInsertStockCurrentStock iInsertStockCurrentStock = new DInsertStockCurrentStock(
                                        stockId,
                                        item.ProductId,
                                        item.PrimaryUnitTypeId,
                                        //item.Quantity,
                                        convertedStock.PrimaryStockQty,
                                        //item.Cost,
                                        convertedStock.PrimaryStockCost,                                        
                                        item.Cost1,
                                        item.Cost2,
                                        locationId,
                                        companyId,
                                        item.ConvertionNo,//ReferenceNo
                                        item.ConvertionDate,//ReferenceDate
                                        item.ProductDimensionId,
                                        item.WareHouseId
                                        //item.GoodsReceiveId,
                                        //item.ImportedStockInId, 
                                        //item.SupplierId
                                    );
                                    iInsertStockCurrentStock.InsertCurrentStock();
                                    if (item.SerialAvailable)
                                    {
                                        foreach (var itemSerial in item.SerialLists.ToList())
                                        {
                                            IInsertStockCurrentStockSerial iInsertStockCurrentStockSerial = new DInsertStockCurrentStockSerial(
                                                stockId,
                                                itemSerial.Serial,
                                                itemSerial.AdditionalSerial
                                                );
                                            iInsertStockCurrentStockSerial.InsertCurrentStockSerial();
                                        }
                                    }
                                }
                            }
                        }
                        transaction.Complete();

                        return new CommonResult()
                        {
                            IsSuccess = isSuccess,
                            Message = "Approve Successful."
                        };
                    }
                    else
                    {
                        return new CommonResult()
                        {
                            IsSuccess = false,
                            Message = "Approve Unsuccessful."
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}