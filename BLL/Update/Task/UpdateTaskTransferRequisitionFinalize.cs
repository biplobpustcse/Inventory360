using Inventory360DataModel;
using Inventory360DataModel.Task;
using Inventory360Entity;
using BLL.Insert.Task;
using DAL.DataAccess.Select.Task;
using DAL.DataAccess.Update.Task;
using DAL.Interface.Select.Task;
using DAL.Interface.Update.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace BLL.Update.Task
{
    public class UpdateTaskTransferRequisitionFinalize
    {
        public CommonResult CancelTransferRequisitionFinalize(Guid id, string reason, long companyId, long userId)
        {
            try
            {
                ISelectTaskTransferRequisitionFinalize iSelectTaskTransferRequisitionFinalize = new DSelectTaskTransferRequisitionFinalize(companyId);
                var selectedRequisitionFinalize = iSelectTaskTransferRequisitionFinalize.SelectStockTransferRequisitionFinalizeAll()
                    .Where(x => x.RequisitionId == id);

                // Check finalize already exist or not
                if (selectedRequisitionFinalize.Count() == 0)
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Selected Transfer Requisition Finalize not found."
                    };
                }

                // Check finalize already approved or not
                if (selectedRequisitionFinalize.Where(x => x.Approved.Equals("A")).Count() > 0)
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Selected Transfer Requisition Finalize already approved."
                    };
                }

                // Check finalize already cancelled or not
                if (selectedRequisitionFinalize.Where(x => x.Approved.Equals("C")).Count() > 0)
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Selected Transfer Requisition Finalize already cancelled."
                    };
                }

                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, ApplicationState.TransactionOptions))
                {
                    IUpdateTaskTransferRequisitionFinalize iUpdateTaskTransferRequisitionFinalize = new DUpdateTaskTransferRequisitionFinalize(id);
                    bool isSuccess = iUpdateTaskTransferRequisitionFinalize.UpdateTransferRequisitionFinalizeForCancel(reason, userId);
                    if (isSuccess)
                    {
                        ISelectTaskTransferRequisitionFinalizeDetail iSelectTaskTransferRequisitionFinalizeDetail = new DSelectTaskTransferRequisitionFinalizeDetail(companyId);
                        var finalizeDetail = iSelectTaskTransferRequisitionFinalizeDetail.SelectRequisitionFinalizeDetailAll()
                            .Where(x => x.RequisitionId == id && x.ItemRequisitionId != null)
                            .Select(sd => new
                            {
                                sd.ItemRequisitionId,
                                sd.ProductId,
                                sd.ProductDimensionId,
                                sd.UnitTypeId,
                                sd.Quantity
                            })
                            .ToList();

                        foreach (var item in finalizeDetail)
                        {
                            // update item requisition detail table in finalized quantity column (-)
                            IUpdateTaskItemRequisitionDetail iUpdateTaskItemRequisitionDetail = new DUpdateTaskItemRequisitionDetail();
                            iUpdateTaskItemRequisitionDetail.UpdateItemRequisitionDetailForFinalizedQuantityDecrease((Guid)item.ItemRequisitionId, item.ProductId, item.UnitTypeId, item.Quantity, item.ProductDimensionId);
                        }

                        // make item requisition not complete
                        var itemRequisitionList = finalizeDetail.Select(s => s.ItemRequisitionId).Distinct().ToList();
                        foreach (Guid requisitionId in itemRequisitionList)
                        {
                            IUpdateTaskItemRequisition iUpdateTaskItemRequisition = new DUpdateTaskItemRequisition(requisitionId);
                            iUpdateTaskItemRequisition.UpdateItemRequisitionForIsSettled(false);
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

        public CommonResult ApproveTransferRequisitionFinalize(Guid id, long companyId, long userId)
        {
            try
            {
                ISelectTaskTransferRequisitionFinalize iSelectTaskTransferRequisitionFinalize = new DSelectTaskTransferRequisitionFinalize(companyId);
                var selectedRequisitionFinalize = iSelectTaskTransferRequisitionFinalize.SelectStockTransferRequisitionFinalizeAll()
                    .Where(x => x.RequisitionId == id);

                // Check finalize already exist or not
                if (selectedRequisitionFinalize.Count() == 0)
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Selected Transfer Requisition Finalize not found."
                    };
                }

                // Check finalize already approved or not
                if (selectedRequisitionFinalize.Where(x => x.Approved.Equals("A")).Count() > 0)
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Selected Transfer Requisition Finalize already approved."
                    };
                }

                // Check finalize already cancelled or not
                if (selectedRequisitionFinalize.Where(x => x.Approved.Equals("C")).Count() > 0)
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Selected Transfer Requisition Finalize already cancelled."
                    };
                }

                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, ApplicationState.TransactionOptions))
                {
                    // update Stock Transfer Requisition as approved
                    IUpdateTaskTransferRequisitionFinalize iUpdateTaskTransferRequisitionFinalize = new DUpdateTaskTransferRequisitionFinalize(id);
                    bool isSuccess = iUpdateTaskTransferRequisitionFinalize.UpdateTransferRequisitionFinalizeForApprove(userId);
                    if (isSuccess)
                    {
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