using Inventory360DataModel;
using Inventory360DataModel.Task;
using DAL.DataAccess.Insert.Stock;
using DAL.DataAccess.Select.Task;
using DAL.DataAccess.Update.Setup;
using DAL.DataAccess.Update.Task;
using DAL.Interface.Insert.Stock;
using DAL.Interface.Select.Task;
using DAL.Interface.Update.Setup;
using DAL.Interface.Update.Task;
using System;
using System.Linq;
using System.Transactions;

namespace BLL.Update.Task
{
    public class UpdateTaskComplainReceive
    {
        public CommonResult CancelComplainReceive(Guid id, string reason, long companyId, long userId)
        {
            try
            {
                ISelectTaskComplainReceive iSelectTaskComplainReceive = new DSelectTaskComplainReceive(companyId);
                var selectedComplainReceive = iSelectTaskComplainReceive.SelectComplainReceiveAll()
                    .Where(x => x.ReceiveId == id);

                // Check Complain Receive already exist or not
                if (selectedComplainReceive.Count() == 0)
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Selected Complain Receive not found."
                    };
                }

                // Check Complain Receive already approved or not
                if (selectedComplainReceive.Where(x => x.Approved.Equals("A")).Count() > 0)
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Selected collection already approved."
                    };
                }

                // Check Complain Receive already cancelled or not
                if (selectedComplainReceive.Where(x => x.Approved.Equals("C")).Count() > 0)
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Selected Complain Receive already cancelled."
                    };
                }

                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, ApplicationState.TransactionOptions))
                {
                    IUpdateTaskComplainReceive iUpdateTaskComplainReceive = new DUpdateTaskComplainReceive(id);
                    bool isSuccess = iUpdateTaskComplainReceive.UpdateComplainReceiveForCancel(reason, userId);
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

        public CommonResult ApproveComplainReceive(Guid id, long companyId, long userId)
        {
            try
            {
                ISelectTaskComplainReceive iSelectTaskComplainReceive = new DSelectTaskComplainReceive(companyId);
                var selectedComplainReceive = iSelectTaskComplainReceive.SelectComplainReceiveAll()
                    .Where(x => x.ReceiveId == id);

                // Check Complain Receive already exist or not
                if (selectedComplainReceive.Count() == 0)
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Selected Complain Receive No not found."
                    };
                }

                // Check Complain Receive already approved or not
                if (selectedComplainReceive.Where(x => x.Approved.Equals("A")).Count() > 0)
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Selected Complain Receive No already approved."
                    };
                }

                // Check Complain Receive already cancelled or not
                if (selectedComplainReceive.Where(x => x.Approved.Equals("C")).Count() > 0)
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Selected Complain Receive No already cancelled."
                    };
                }

                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, ApplicationState.TransactionOptions))
                {
                    // update Complain Receive as approved
                    IUpdateTaskComplainReceive iUpdateTaskComplainReceive = new DUpdateTaskComplainReceive(id);
                    bool isSuccess = iUpdateTaskComplainReceive.UpdateComplainReceiveForApprove(userId);
                    if (isSuccess)
                    {
                        ISelectTaskComplainReceiveDetail iSelectTaskComplainReceiveDetail = new DSelectTaskComplainReceiveDetail(companyId);
                        var detailLists = iSelectTaskComplainReceiveDetail.SelectTaskComplainReceiveDetailAll()
                            .Where(x => x.Task_ComplainReceive.ReceiveId == id)
                            .Select(s => new
                            {
                                s.Task_ComplainReceive.ReceiveNo,
                                s.Task_ComplainReceive.ReceiveDate,
                                s.Task_ComplainReceive.LocationId,
                                s.Serial,
                                s.AdditionalSerial,
                                s.ProductId,
                                s.ProductDimensionId,
                                s.UnitTypeId,
                                Quantity = 1,
                                s.Cost,
                                s.Cost1,
                                s.Cost2
                            })
                            .ToList();

                        foreach (var item in detailLists)
                        {
                            var RMAStockId = Guid.NewGuid();

                            IInsertStockRMAStock iInsertStockRMAStock = new DInsertStockRMAStock(RMAStockId,item.ReceiveNo, item.ReceiveDate, item.ProductId, item.UnitTypeId, item.Quantity, item.Cost, item.Cost1, item.Cost2, item.LocationId, companyId, item.ProductDimensionId, null);
                            iInsertStockRMAStock.InsertRMAStock();

                            IInsertStockRMAStockSerial iInsertStockRMAStockSerial = new DInsertStockRMAStockSerial(RMAStockId, item.Serial, item.AdditionalSerial);
                            iInsertStockRMAStockSerial.InsertRMAStockSerial();

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