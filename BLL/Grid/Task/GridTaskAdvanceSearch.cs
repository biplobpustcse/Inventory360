using Inventory360DataModel;
using DAL.DataAccess.Select.Stock;
using DAL.DataAccess.Select.Task;
using DAL.Interface.Select.Stock;
using DAL.Interface.Select.Task;
using System;
using System.Linq;

namespace BLL.Grid.Task
{
    public class GridTaskAdvanceSearch
    {
        private CommonAdvanceSearch<dynamic> SelectAdvanceSearchData(string serial, long companyId)
        {
            try
            {
                var data = new CommonAdvanceSearch<dynamic>();
                
                #region 0.1 Stock position(Current)
                ISelectStockCurrentStockSerial iSelectStockCurrentStockSerial = new DSelectStockCurrentStockSerial(companyId);
                var currentStockLists = iSelectStockCurrentStockSerial.SelectCurrentStockSerialAll()
                    .Where(x => x.Serial == serial)
                    .Select(s => new
                    {
                        StockId = s.Stock_CurrentStock.CurrentStockId,
                        Serial = s.Serial,
                        ProductId = s.Stock_CurrentStock.ProductId,
                        ProductName = s.Stock_CurrentStock.Setup_Product.Name,
                        ProductCode = s.Stock_CurrentStock.Setup_Product.Code,
                        StockLocation = s.Stock_CurrentStock.Setup_Location.Name,
                        WareHouse = s.Stock_CurrentStock.Setup_Location1.Name ?? "N/A",
                        StockType = "Current",
                        WarrantyPeriod = s.Stock_CurrentStock.Setup_Product.IsLifeTimeWarranty == true? "Life Time Warranty" : s.Stock_CurrentStock.Setup_Product.WarrantyDays.ToString()+" Days" ,
                        ServiceWarrantyPeriod = s.Stock_CurrentStock.Setup_Product.IsLifeTimeWarranty == true? "Life Time Warranty" : s.Stock_CurrentStock.Setup_Product.ServiceWarrantyDays.ToString()+" Days"                                                
                    }).ToList();
                #endregion

                #region 0.2 Stock position(RMA)
                ISelectStockRMAStockSerial ISelectStockRMAStockSerial = new DSelectStockRMAStockSerial(companyId);
                var rmaStockLists = ISelectStockRMAStockSerial.SelectRMAStockSerialAll()
                    .Where(x => x.Serial == serial)
                    .Select(s => new
                    {
                        StockId = s.Stock_RMAStock.RMAStockId,
                        Serial = s.Serial,
                        ProductId = s.Stock_RMAStock.ProductId,
                        ProductName = s.Stock_RMAStock.Setup_Product.Name,
                        ProductCode = s.Stock_RMAStock.Setup_Product.Code,
                        StockLocation = s.Stock_RMAStock.Setup_Location.Name,
                        WareHouse = s.Stock_RMAStock.Setup_Location1.Name ?? "N/A",
                        StockType = "RMA",
                        WarrantyPeriod = s.Stock_RMAStock.Setup_Product.IsLifeTimeWarranty == true ? "Life Time Warranty" : s.Stock_RMAStock.Setup_Product.WarrantyDays.ToString() + " Days",
                        ServiceWarrantyPeriod = s.Stock_RMAStock.Setup_Product.IsLifeTimeWarranty == true ? "Life Time Warranty" : s.Stock_RMAStock.Setup_Product.ServiceWarrantyDays.ToString() + " Days"
                    }).ToList();
                #endregion

                #region 0.3 Stock position(Bad)
                ISelectStockBadStockSerial iSelectStockBadStockSerial = new DSelectStockBadStockSerial(companyId);
                var badStockLists = iSelectStockBadStockSerial.SelectBadStockSerialAll()
                    .Where(x => x.Serial == serial)
                    .Select(s => new
                    {
                        StockId = s.Stock_BadStock.BadStockId,
                        Serial = s.Serial,
                        ProductId = s.Stock_BadStock.ProductId,
                        ProductName = s.Stock_BadStock.Setup_Product.Name,
                        ProductCode = s.Stock_BadStock.Setup_Product.Code,
                        StockLocation = s.Stock_BadStock.Setup_Location.Name,
                        WareHouse = s.Stock_BadStock.Setup_Location1.Name ?? "N/A",
                        StockType = "Bad",
                        WarrantyPeriod = s.Stock_BadStock.Setup_Product.IsLifeTimeWarranty == true ? "Life Time Warranty" : s.Stock_BadStock.Setup_Product.WarrantyDays.ToString() + " Days",
                        ServiceWarrantyPeriod = s.Stock_BadStock.Setup_Product.IsLifeTimeWarranty == true ? "Life Time Warranty" : s.Stock_BadStock.Setup_Product.ServiceWarrantyDays.ToString() + " Days"
                    }).ToList();
                #endregion
                data.stockLists = currentStockLists
                    .Union(rmaStockLists)
                    .Union(badStockLists)
                    .Select(x=> new {
                        x.StockId,
                        x.Serial,
                        x.ProductId,
                        x.ProductName,
                        x.ProductCode,
                        x.StockLocation,
                        x.WareHouse,
                        x.StockType,
                        x.WarrantyPeriod,
                        x.ServiceWarrantyPeriod,
                    }).ToList();


                #region 1. Purchase History
                ISelectTaskGoodsReceiveDetailSerial iSelectTaskGoodsReceiveDetailSerial = new DSelectTaskGoodsReceiveDetailSerial(companyId);
                var purchaseLists = iSelectTaskGoodsReceiveDetailSerial.SelectGoodsReceiveDetailSerialAll()
                    .Where(x => x.Serial == serial)
                    .Where(x=>x.Task_GoodsReceiveDetail.Task_GoodsReceive.Approved == "A")
                    .Select(s => new
                    {                        
                        ProductId = s.Task_GoodsReceiveDetail.ProductId,
                        Serial = s.Serial,
                        ProductName = s.Task_GoodsReceiveDetail.Setup_Product.Name,
                        ProductCode = s.Task_GoodsReceiveDetail.Setup_Product.Code,
                        OrderId = s.Task_GoodsReceiveDetail.Task_GoodsReceive.OrderId,
                        OrderNo = s.Task_GoodsReceiveDetail.Task_GoodsReceive.Task_PurchaseOrder.OrderNo,
                        OrderDate = s.Task_GoodsReceiveDetail.Task_GoodsReceive.Task_PurchaseOrder.OrderDate,
                        SupplierName = s.Task_GoodsReceiveDetail.Task_GoodsReceive.Task_PurchaseOrder.Setup_Supplier.Name,
                        LocationName = s.Task_GoodsReceiveDetail.Task_GoodsReceive.Task_PurchaseOrder.Setup_Location.Name,
                        EntryBy = s.Task_GoodsReceiveDetail.Task_GoodsReceive.Task_PurchaseOrder.Security_User1.FullName,
                        WareHouse = s.Task_GoodsReceiveDetail.Setup_Location.Name ?? "N/A",
                        ReferenceNo = s.Task_GoodsReceiveDetail.Task_GoodsReceive.Task_PurchaseOrder.ReferenceNo
                    });
                data.purchaseLists = purchaseLists.ToList();
                #endregion

                #region 2. Purchase Return
                ISelectTaskPurchaseReturnDetailSerial iSelectTaskPurchaseReturnDetailSerial = new DSelectTaskPurchaseReturnDetailSerial(companyId);
                var purchaseReturnLists = iSelectTaskPurchaseReturnDetailSerial.SelectPurchaseReturnDetailSerialAll()
                    .Where(x => x.Serial == serial)
                    .Where(x => x.Task_PurchaseReturnDetail.Task_PurchaseReturn.Approved == "A")
                    .Select(s => new
                    {
                        ProductId = s.Task_PurchaseReturnDetail.ProductId,
                        Serial = s.Serial,
                        ProductName = s.Task_PurchaseReturnDetail.Setup_Product.Name,
                        ProductCode = s.Task_PurchaseReturnDetail.Setup_Product.Code,
                        ReturnId = s.Task_PurchaseReturnDetail.ReturnId,
                        ReturnNo = s.Task_PurchaseReturnDetail.Task_PurchaseReturn.ReturnNo,
                        ReturnDate = s.Task_PurchaseReturnDetail.Task_PurchaseReturn.ReturnDate,
                        SupplierName = s.Task_PurchaseReturnDetail.Task_PurchaseReturn.Setup_Supplier.Name,
                        LocationName = s.Task_PurchaseReturnDetail.Task_PurchaseReturn.Setup_Location.Name,
                        EntryBy = s.Task_PurchaseReturnDetail.Task_PurchaseReturn.Security_User1.FullName,
                        WareHouse = s.Task_PurchaseReturnDetail.Task_PurchaseReturn.Setup_Location.Name ?? "N/A",
                        PurchaseNumber = s.Task_PurchaseReturnDetail.Task_GoodsReceive.Task_PurchaseOrder.OrderNo,
                        PurchaseDate = s.Task_PurchaseReturnDetail.Task_GoodsReceive.Task_PurchaseOrder.OrderDate
                    });
                data.purchaseReturnLists = purchaseReturnLists.ToList();
                #endregion

                #region 3. Import In History
                ISelectTaskImportedStockInDetailSerial iSelectTaskImportedStockInDetailSerial = new DSelectTaskImportedStockInDetailSerial(companyId);
                var importInLists = iSelectTaskImportedStockInDetailSerial.SelectImportedStockInDetailSerialAll()
                    .Where(x => x.Serial == serial)
                    .Where(x => x.Task_ImportedStockInDetail.Task_ImportedStockIn.Approved == "A")
                    .Select(s => new
                    {
                        ProductId = s.Task_ImportedStockInDetail.ProductId,
                        Serial = s.Serial,
                        ProductName = s.Task_ImportedStockInDetail.Setup_Product.Name,
                        ProductCode = s.Task_ImportedStockInDetail.Setup_Product.Code,
                        StockInId = s.Task_ImportedStockInDetail.StockInId,
                        StockInNo = s.Task_ImportedStockInDetail.Task_ImportedStockIn.StockInNo,
                        StockInDate = s.Task_ImportedStockInDetail.Task_ImportedStockIn.StockInDate,
                        SupplierName = s.Task_ImportedStockInDetail.Task_ImportedStockIn.Task_LIMStockIn.Task_ProformaInvoice.Setup_Supplier.Name,
                        LocationName = s.Task_ImportedStockInDetail.Task_ImportedStockIn.Setup_Location.Name,
                        EntryBy = s.Task_ImportedStockInDetail.Task_ImportedStockIn.Security_User1.FullName,
                        WareHouse = s.Task_ImportedStockInDetail.Setup_Location.Name ?? "N/A",
                        LCNumber = s.Task_ImportedStockInDetail.Task_ImportedStockIn.Task_LIMStockIn.Task_ProformaInvoice.Task_LCOpening.FirstOrDefault().LCNo,
                        LCDate = s.Task_ImportedStockInDetail.Task_ImportedStockIn.Task_LIMStockIn.Task_ProformaInvoice.Task_LCOpening.FirstOrDefault().LCDate,                       
                        PINumber = s.Task_ImportedStockInDetail.Task_ImportedStockIn.Task_LIMStockIn.Task_ProformaInvoice.InvoiceNo,
                        PIDate = s.Task_ImportedStockInDetail.Task_ImportedStockIn.Task_LIMStockIn.Task_ProformaInvoice.InvoiceDate
                    });
                data.importInLists = importInLists.ToList();
                #endregion

                #region 4. Atock Adjustment History
                ISelectTaskStockAdjustmentDetailSerial iSelectTaskStockAdjustmentDetailSerial = new DSelectTaskStockAdjustmentDetailSerial(companyId);
                var stockAdjustmentLists = iSelectTaskStockAdjustmentDetailSerial.SelectStockAdjustmentDetailSerialAll()
                    .Where( x => x.Serial == serial)
                    .Where(x => x.Task_StockAdjustmentDetail.Task_StockAdjustment.Approved == "A")
                    .Select(s => new
                    {
                        ProductId = s.Task_StockAdjustmentDetail.ProductId,
                        Serial = s.Serial,
                        ProductName = s.Task_StockAdjustmentDetail.Setup_Product.Name,
                        ProductCode = s.Task_StockAdjustmentDetail.Setup_Product.Code,
                        AdjustmentId = s.Task_StockAdjustmentDetail.AdjustmentId,
                        AdjustmentNo = s.Task_StockAdjustmentDetail.Task_StockAdjustment.AdjustmentNo,
                        AdjustmentDate = s.Task_StockAdjustmentDetail.Task_StockAdjustment.AdjustmentDate,
                        AdjustmentType = s.Task_StockAdjustmentDetail.IncreaseDecrease == "I" ? "Addition" : "Deduction",
                        SerialNumber = s.Serial,
                        EntryBy = s.Task_StockAdjustmentDetail.Task_StockAdjustment.Security_User1.FullName                     
                    });
                data.stockAdjustmentLists = stockAdjustmentLists.ToList();
                #endregion

                #region 5. sales Invoice
                ISelectTaskSalesInvoiceDetailSerial iSelectTaskSalesInvoiceDetailSerial = new DSelectTaskSalesInvoiceDetailSerial(companyId);
                var salesInvoiceList = iSelectTaskSalesInvoiceDetailSerial.SelectSalesInvoiceDetailSerialAll()
                    .Where(x => x.Serial == serial)
                    .Where(x => x.Task_SalesInvoiceDetail.Task_SalesInvoice.Approved == "A")
                    .Select(s => new 
                    {
                        ProductId = s.Task_SalesInvoiceDetail.ProductId,
                        Serial = s.Serial,
                        ProductName = s.Task_SalesInvoiceDetail.Setup_Product.Name,
                        ProductCode = s.Task_SalesInvoiceDetail.Setup_Product.Code,
                        InvoiceId = s.Task_SalesInvoiceDetail.InvoiceId,
                        InvoiceNo = s.Task_SalesInvoiceDetail.Task_SalesInvoice.InvoiceNo,
                        InvoiceDate = s.Task_SalesInvoiceDetail.Task_SalesInvoice.InvoiceDate,                        
                        CustomerName = s.Task_SalesInvoiceDetail.Task_SalesInvoice.Setup_Customer.Name,                             
                        LocationName = s.Task_SalesInvoiceDetail.Task_SalesInvoice.Setup_Location.Name,
                        EntryBy = s.Task_SalesInvoiceDetail.Task_SalesInvoice.Security_User1.FullName,
                        WareHouse = s.Task_SalesInvoiceDetail.Task_DeliveryChallan.Task_DeliveryChallanDetail.Where(x=>x.ProductId == s.Task_SalesInvoiceDetail.ProductId && x.ProductDimensionId == s.Task_SalesInvoiceDetail.ProductDimensionId && x.UnitTypeId == s.Task_SalesInvoiceDetail.UnitTypeId).FirstOrDefault().Setup_Location.Name ??"N/A",
                        PaymentPromiseDate = s.Task_SalesInvoiceDetail.Task_DeliveryChallan.Task_SalesOrder.PromisedDate
                    });
                data.salesInvoiceList = salesInvoiceList.ToList();
                #endregion

                #region 6. Sales Return
                ISelectTaskSalesReturnDetailSerial iSelectTaskSalesReturnDetailSerial = new DSelectTaskSalesReturnDetailSerial(companyId);
                var salesReturnLists = iSelectTaskSalesReturnDetailSerial.SelectSalesReturnDetailSerialAll()
                    .Where(x => x.Serial == serial)
                    .Where(x => x.Task_SalesReturnDetail.Task_SalesReturn.Approved == "A")
                    .Select(s => new
                    {
                        ProductId = s.Task_SalesReturnDetail.ProductId,
                        Serial = s.Serial,
                        ProductName = s.Task_SalesReturnDetail.Setup_Product.Name,
                        ProductCode = s.Task_SalesReturnDetail.Setup_Product.Code,
                        ReturnId = s.Task_SalesReturnDetail.ReturnId,
                        ReturnNo = s.Task_SalesReturnDetail.Task_SalesReturn.ReturnNo,
                        ReturnDate = s.Task_SalesReturnDetail.Task_SalesReturn.ReturnDate,
                        CustomerName = s.Task_SalesReturnDetail.Task_SalesReturn.Setup_Customer.Name,
                        LocationName = s.Task_SalesReturnDetail.Task_SalesReturn.Setup_Location.Name,
                        EntryBy = s.Task_SalesReturnDetail.Task_SalesReturn.Security_User1.FullName,
                        WareHouse = s.Task_SalesReturnDetail.Task_SalesInvoice.Task_SalesInvoiceDetail.Where(x => x.ProductId == s.Task_SalesReturnDetail.ProductId && x.ProductDimensionId == s.Task_SalesReturnDetail.ProductDimensionId && x.UnitTypeId == s.Task_SalesReturnDetail.UnitTypeId).FirstOrDefault().Setup_Location.Name ?? "N/A",
                        InvoiceNumber = s.Task_SalesReturnDetail.Task_SalesInvoice.InvoiceNo,
                        InvoiceDate = s.Task_SalesReturnDetail.Task_SalesInvoice.InvoiceDate
                    });
                data.salesReturnLists = salesReturnLists.ToList();
                #endregion

                #region 7. Stock Transfer History (Location Transfer)
                ISelectTaskTransferChallanDetailSerial iSelectTaskTransferChallanDetailSerial = new DSelectTaskTransferChallanDetailSerial(companyId);
                var stockTransferLists = iSelectTaskTransferChallanDetailSerial.SelectTransferChallanDetailSerialAll()
                    .Where(x => x.Serial == serial)
                    .Where(x=>x.Task_TransferChallanDetail.Task_TransferChallan.LocationId != x.Task_TransferChallanDetail.Task_TransferChallan.TransferToId)
                    .Where(x => x.Task_TransferChallanDetail.Task_TransferChallan.Approved == "A")
                    .Select(s => new
                    {
                        ProductId = s.Task_TransferChallanDetail.ProductId,
                        Serial = s.Serial,
                        ProductName = s.Task_TransferChallanDetail.Setup_Product.Name,
                        ProductCode = s.Task_TransferChallanDetail.Setup_Product.Code,
                        ChallanId = s.Task_TransferChallanDetail.ChallanId,
                        ChallanNo = s.Task_TransferChallanDetail.Task_TransferChallan.ChallanNo,
                        ChallanDate = s.Task_TransferChallanDetail.Task_TransferChallan.ChallanDate,
                        SerialNo = s.Serial,                       
                        FromLocationName = s.Task_TransferChallanDetail.Task_TransferChallan.Setup_Location.Name,
                        WareHouse = s.Task_TransferChallanDetail.Setup_Location.Name?? "N/A",
                        TransferedBy = s.Task_TransferChallanDetail.Task_TransferChallan.Security_User.FullName,
                        FromStockType = s.Task_TransferChallanDetail.Task_TransferChallan.TransferFromStockType,
                        ToLocationName = s.Task_TransferChallanDetail.Task_TransferChallan.Setup_Location1.Name,
                        ToStockType = s.Task_TransferChallanDetail.Task_TransferChallan.TransferToStockType,
                        ReceiveNumber = s.Task_TransferChallanDetail.Task_TransferChallan.Task_TransferReceive.FirstOrDefault().ReceiveNo,
                        ReceivedBy = s.Task_TransferChallanDetail.Task_TransferChallan.Task_TransferReceive.FirstOrDefault().EntryBy
                    });
                data.stockTransferLists = stockTransferLists.ToList();
                #endregion

                #region 8. Internal Stock Transfer History               
                var internalStockTransferLists = iSelectTaskTransferChallanDetailSerial.SelectTransferChallanDetailSerialAll()
                    .Where(x => x.Serial == serial)
                    .Where(x => x.Task_TransferChallanDetail.Task_TransferChallan.LocationId == x.Task_TransferChallanDetail.Task_TransferChallan.TransferToId)
                    .Where(x => x.Task_TransferChallanDetail.Task_TransferChallan.Approved == "A")
                    .Select(s => new
                    {
                        ProductId = s.Task_TransferChallanDetail.ProductId,
                        Serial = s.Serial,
                        ProductName = s.Task_TransferChallanDetail.Setup_Product.Name,
                        ProductCode = s.Task_TransferChallanDetail.Setup_Product.Code,
                        ChallanId = s.Task_TransferChallanDetail.ChallanId,
                        ChallanNo = s.Task_TransferChallanDetail.Task_TransferChallan.ChallanNo,
                        ChallanDate = s.Task_TransferChallanDetail.Task_TransferChallan.ChallanDate,
                        SerialNo = s.Serial,
                        FromLocationName = s.Task_TransferChallanDetail.Task_TransferChallan.Setup_Location.Name,
                        WareHouse = s.Task_TransferChallanDetail.Setup_Location.Name?? "N/A",
                        TransferedBy = s.Task_TransferChallanDetail.Task_TransferChallan.Security_User.FullName,
                        FromStockType = s.Task_TransferChallanDetail.Task_TransferChallan.TransferFromStockType,
                        ToLocationName = s.Task_TransferChallanDetail.Task_TransferChallan.Setup_Location1.Name,
                        ToStockType = s.Task_TransferChallanDetail.Task_TransferChallan.TransferToStockType,
                        ReceiveNumber = s.Task_TransferChallanDetail.Task_TransferChallan.Task_TransferReceive.FirstOrDefault().ReceiveNo,
                        ReceivedBy = s.Task_TransferChallanDetail.Task_TransferChallan.Task_TransferReceive.FirstOrDefault().EntryBy
                    });
                data.internalStockTransferLists = internalStockTransferLists.ToList();
                #endregion

                #region 9. Complain Receive
                ISelectTaskComplainReceiveDetail iSelectTaskComplainReceiveDetail = new DSelectTaskComplainReceiveDetail(companyId);
                var complainReceiveLists = iSelectTaskComplainReceiveDetail.SelectTaskComplainReceiveDetailAll()
                    .Where(x => x.Serial == serial)
                    .Where(x => x.Task_ComplainReceive.Approved == "A")
                    .Select(s => new
                    {
                        ProductId = s.ProductId,
                        Serial = s.Serial,
                        ProductName = s.Setup_Product.Name,
                        ProductCode = s.Setup_Product.Code,
                        ReceiveId = s.Task_ComplainReceive.ReceiveId,
                        ReceiveNo = s.Task_ComplainReceive.ReceiveNo,
                        ReceiveDate = s.Task_ComplainReceive.ReceiveDate,
                        ReceiveSerial = s.Serial,
                        CustomerName = s.Task_ComplainReceive.Setup_Customer.Name,
                        LocationName = s.Task_ComplainReceive.Setup_Location.Name,
                        EntryBy = s.Task_ComplainReceive.Security_User1.FullName,                        
                        InvoiceNumber = s.Task_SalesInvoice.InvoiceNo,
                        InvoiceDate = s.Task_SalesInvoice.InvoiceDate
                    });
                data.complainReceiveLists = complainReceiveLists.ToList();
                #endregion

                #region 10. Replacement Claim to Vendor
                ISelectTaskReplacementClaimDetail iSelectTaskReplacementClaimDetail = new DSelectTaskReplacementClaimDetail(companyId);
                var replacementClaimLists = iSelectTaskReplacementClaimDetail.SelectReplacementClaimDetailAll()
                    .Where(x => x.Serial == serial)
                    .Where(x => x.Task_ReplacementClaim.Approved == "A")
                    .Select(s => new
                    {
                        ProductId = s.ProductId,
                        Serial = s.Serial,
                        ProductName = s.Setup_Product.Name,
                        ProductCode = s.Setup_Product.Code,
                        ClaimId = s.Task_ReplacementClaim.ClaimId,
                        ClaimNo = s.Task_ReplacementClaim.ClaimNo,
                        ClaimDate = s.Task_ReplacementClaim.ClaimDate,
                        ClaimSerial = s.Serial,
                        SupplierName = s.Task_ReplacementClaim.Setup_Supplier.Name,
                        LocationName = s.Task_ReplacementClaim.Setup_Location.Name,
                        EntryBy = s.Task_ReplacementClaim.Security_User1.FullName,
                        ComplainNumber = s.Task_ComplainReceive.ReceiveNo,
                        ComplainDate = s.Task_ComplainReceive.ReceiveDate
                    });
                data.replacementClaimLists = replacementClaimLists.ToList();
                #endregion

                #region 11. Receive against Replacement Claim
                ISelectTaskReplacementReceiveDetail iSelectTaskReplacementReceiveDetail = new DSelectTaskReplacementReceiveDetail(companyId);
                var replacementReceiveLists = iSelectTaskReplacementReceiveDetail.SelectReplacementReceiveDetailAll()
                    .Where(x => x.PreviousSerial == serial || x.NewSerial == serial)
                    .Where(x => x.Task_ReplacementReceive.Approved == "A")
                    .Select(s => new
                    {
                        ProductId = s.NewProductId,
                        ProductName = s.Setup_Product.Name,
                        ProductCode = s.Setup_Product.Code,
                        ReceiveId = s.Task_ReplacementReceive.ReceiveId,
                        ReceiveNo = s.Task_ReplacementReceive.ReceiveNo,
                        ReceiveDate = s.Task_ReplacementReceive.ReceiveDate,
                        PreviousSerial = s.PreviousSerial,
                        NewSerial = s.NewSerial,
                        SupplierName = s.Task_ReplacementClaim.Setup_Supplier.Name,
                        LocationName = s.Task_ReplacementReceive.Setup_Location.Name,
                        EntryBy = s.Task_ReplacementClaim.Security_User1.FullName,
                        ClaimNumber = s.Task_ReplacementClaim.ClaimNo,
                        ClaimDate = s.Task_ReplacementClaim.ClaimDate
                    });
                data.replacementReceiveLists = replacementReceiveLists.ToList();
                #endregion

                #region 12. Customer Delivery History
                ISelectTaskCustomerDeliveryDetail iSelectTaskCustomerDeliveryDetail = new DSelectTaskCustomerDeliveryDetail(companyId);
                var customerDeliveryLists = iSelectTaskCustomerDeliveryDetail.SelectCustomerDeliveryDetailAll()
                    .Where(x => x.PreviousSerial == serial || x.NewSerial == serial)
                    .Where(x => x.Task_CustomerDelivery.Approved == "A")
                    .Select(s => new
                    {
                        ProductId = s.NewProductId,
                        ProductName = s.Setup_Product.Name,
                        ProductCode = s.Setup_Product.Code,
                        DeliveryId = s.Task_CustomerDelivery.DeliveryId,
                        DeliveryNo = s.Task_CustomerDelivery.DeliveryNo,
                        DeliveryDate = s.Task_CustomerDelivery.DeliveryDate,
                        PreviousSerial = s.PreviousSerial,
                        NewSerial = s.NewSerial,
                        CustomerName = s.Task_CustomerDelivery.Setup_Customer.Name,
                        LocationName = s.Task_CustomerDelivery.Setup_Location.Name,
                        EntryBy = s.Task_CustomerDelivery.Security_User1.FullName,
                        ComplainNumber = s.Task_ComplainReceive.ReceiveNo,
                        ComplainDate = s.Task_ComplainReceive.ReceiveDate
                        
                    });
                data.customerDeliveryLists = customerDeliveryLists.ToList();
                #endregion

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object SelectAdvanceSearchDataLists(string serial, long companyId)
        {
            try
            {
                return SelectAdvanceSearchData(serial, companyId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}