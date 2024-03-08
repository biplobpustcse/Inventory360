using Inventory360DataModel;
using BLL.Common;
using DAL.DataAccess.Select.Task;
using DAL.DataAccess.Update.Setup;
using DAL.DataAccess.Update.Task;
using DAL.Interface.Select.Task;
using DAL.Interface.Update.Setup;
using DAL.Interface.Update.Task;
using System;
using System.Linq;
using System.Transactions;

namespace BLL.Update.Task
{
    public class UpdateTaskCollection
    {
        public CommonResult CancelCollection(Guid id, string reason, long companyId, long userId)
        {
            try
            {
                ISelectTaskCollection iSelectTaskCollection = new DSelectTaskCollection(companyId);
                var selectedCollection = iSelectTaskCollection.SelectCollectionAll()
                    .Where(x => x.CollectionId == id);

                // Check collection already exist or not
                if (selectedCollection.Count() == 0)
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Selected collection not found."
                    };
                }

                // Check collection already approved or not
                if (selectedCollection.Where(x => x.Approved.Equals("A")).Count() > 0)
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Selected collection already approved."
                    };
                }

                // Check collection already cancelled or not
                if (selectedCollection.Where(x => x.Approved.Equals("C")).Count() > 0)
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Selected collection already cancelled."
                    };
                }

                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, ApplicationState.TransactionOptions))
                {
                    IUpdateTaskCollection iUpdateTaskCollection = new DUpdateTaskCollection(id);
                    bool isSuccess = iUpdateTaskCollection.UpdateCollectionForCancel(reason, userId);
                    if (isSuccess)
                    {
                        ISelectTaskCollectionMapping iSelectTaskCollectionMapping = new DSelectTaskCollectionMapping(companyId);
                        var collectionMapping = iSelectTaskCollectionMapping.SelectCollectionMappingAll()
                            .Where(x => x.CollectionId == id)
                            .Select(sd => new
                            {
                                sd.Task_Collection.CustomerId,
                                sd.SalesOrderId,
                                sd.InvoiceId,
                                sd.Amount,
                                sd.Amount1,
                                sd.Amount2
                            })
                            .ToList();

                        foreach (var item in collectionMapping)
                        {
                            CurrencyConvertedAmount amount = new CurrencyConvertedAmount
                            {
                                BaseAmount = item.Amount,
                                Currency1Amount = item.Amount1,
                                Currency2Amount = item.Amount2
                            };

                            if (item.SalesOrderId != null)
                            {
                                // update sales order table in collectedAmount column (-)
                                IUpdateTaskSalesOrder iUpdateTaskSalesOrder = new DUpdateTaskSalesOrder((Guid)item.SalesOrderId);
                                iUpdateTaskSalesOrder.UpdateSalesOrderByCollectionInCollectedAmountDecrease(amount);
                            }
                            else if (item.InvoiceId != null)
                            {
                                // update sales invoice table in collectedamount column (-)
                                IUpdateTaskSalesInvoice iUpdateTaskSalesInvoice = new DUpdateTaskSalesInvoice((Guid)item.InvoiceId);
                                iUpdateTaskSalesInvoice.UpdateSalesInvoiceByCollectedAmountDecrease(amount);

                                // update sales invoice table in issettled column
                                ISelectTaskSalesInvoice iSelectTaskSalesInvoice = new DSelectTaskSalesInvoice(companyId);
                                bool isSettled = iSelectTaskSalesInvoice.CheckSalesInvoiceIsSettledByCollection((Guid)item.InvoiceId);
                                iUpdateTaskSalesInvoice.UpdateSalesInvoiceForIsSettled(isSettled);
                            }
                            else
                            {
                                // update customer table in collected amount column (-)
                                IUpdateSetupCustomer iUpdateSetupCustomer = new DUpdateSetupCustomer(item.CustomerId);
                                iUpdateSetupCustomer.UpdateCustomerOpeningByCollectionInCollectedAmountAsDecrease(amount);
                            }
                        }

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

        public CommonResult ApproveCollection(Guid id, long companyId, long userId)
        {
            try
            {
                ISelectTaskCollection iSelectTaskCollection = new DSelectTaskCollection(companyId);
                var selectedCollection = iSelectTaskCollection.SelectCollectionAll()
                    .Where(x => x.CollectionId == id);

                // Check collection already exist or not
                if (selectedCollection.Count() == 0)
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Selected collection not found."
                    };
                }

                // Check collection already approved or not
                if (selectedCollection.Where(x => x.Approved.Equals("A")).Count() > 0)
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Selected collection already approved."
                    };
                }

                // Check collection already cancelled or not
                if (selectedCollection.Where(x => x.Approved.Equals("C")).Count() > 0)
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Selected collection already cancelled."
                    };
                }

                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, ApplicationState.TransactionOptions))
                {
                    var collectionInfo = selectedCollection.Select(s => new
                    {
                        s.CollectionDate,
                        s.SelectedCurrency,
                        s.CustomerId,
                        s.Currency1Rate,
                        s.Currency2Rate,
                        s.LocationId
                    })
                    .FirstOrDefault();

                    // update collection as approved
                    IUpdateTaskCollection iUpdateTaskCollection = new DUpdateTaskCollection(id);
                    bool isSuccess = iUpdateTaskCollection.UpdateCollectionForApprove(userId);
                    if (isSuccess)
                    {
                        ISelectTaskCollectionDetail iSelectTaskCollectionDetail = new DSelectTaskCollectionDetail(companyId);
                        var collectionDetail = iSelectTaskCollectionDetail.SelectCollectionDetailAll()
                            .Where(x => x.CollectionId == id)
                            .Select(s => new
                            {
                                s.CollectionDetailId,
                                s.PaymentModeId,
                                s.Amount,
                                s.Amount1,
                                s.Amount2
                            })
                            .ToList();

                        foreach (var item in collectionDetail)
                        {
                            // save collection voucher into voucher and voucher detail table
                            Guid voucherId = Guid.Empty;

                            GenerateAutoVoucher generateAutoVoucher = new GenerateAutoVoucher();
                            bool isVoucherGenerated = generateAutoVoucher.GenerateAutoVoucherForEvent(
                                collectionInfo.SelectedCurrency,
                                CommonEnum.OperationalEvent.Sales.ToString(),
                                CommonEnum.OperationalSubEvent.Collection.ToString(),
                                CommonEnum.OperationType.Regular.ToString(),
                                item.PaymentModeId,
                                collectionInfo.CollectionDate.Date,
                                item.Amount,
                                collectionInfo.Currency1Rate,
                                item.Amount1,
                                collectionInfo.Currency2Rate,
                                item.Amount2,
                                collectionInfo.LocationId,
                                companyId,
                                userId,
                                collectionInfo.CustomerId,
                                out voucherId);

                            if (isVoucherGenerated)
                            {
                                // update voucher id into collection detail table
                                IUpdateTaskCollectionDetail iUpdateTaskCollectionDetail = new DUpdateTaskCollectionDetail(item.CollectionDetailId);
                                iUpdateTaskCollectionDetail.UpdateCollectionDetailForVoucherId(voucherId);
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