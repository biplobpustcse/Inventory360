using Inventory360DataModel;
using DAL.DataAccess.Select.Task;
using DAL.DataAccess.Update.Task;
using DAL.Interface.Select.Task;
using DAL.Interface.Update.Task;
using System;
using System.Linq;
using System.Transactions;

namespace BLL.Update.Task
{
    public class UpdateTaskTransferOrder
    {
        public CommonResult CancelTransferOrder(Guid id, string reason, long companyId, long userId)
        {
            try
            {
                ISelectTaskTransferOrder iSelectTaskTransferOrder = new DSelectTaskTransferOrder(companyId);
                var selectedTransferOrder = iSelectTaskTransferOrder.SelectTaskTransferOrder(id)
                    .Where(x => x.OrderId == id);

                // Check finalize already exist or not
                if (selectedTransferOrder.Count() == 0)
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Selected Transfer Order not found."
                    };
                }

                // Check finalize already approved or not
                if (selectedTransferOrder.Where(x => x.Approved.Equals("A")).Count() > 0)
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Selected Transfer Order already approved."
                    };
                }

                // Check finalize already cancelled or not
                if (selectedTransferOrder.Where(x => x.Approved.Equals("C")).Count() > 0)
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Selected Transfer Order already cancelled."
                    };
                }

                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, ApplicationState.TransactionOptions))
                {
                    IUpdateTaskTransferOrder iUpdateTaskTransferOrder = new DUpdateTaskTransferOrder(id);
                    bool isSuccess = iUpdateTaskTransferOrder.UpdateTransferOrderForCancel(reason, userId);
                    if (isSuccess)
                    {
                        // should decrease quantity into Task_TransferRequisitionFinalizeDetail table
                        ISelectTaskTransferOrderDetail iSelectTaskTransferOrderDetail = new DSelectTaskTransferOrderDetail(companyId);
                        var detailLists = iSelectTaskTransferOrderDetail.SelectTransferOrderDetailAll()
                            .Where(x => x.RequisitionId != null
                                && x.OrderId == id)
                            .Select(s => new
                            {
                                s.RequisitionId,
                                s.ProductId,
                                s.ProductDimensionId,
                                s.UnitTypeId,
                                s.Quantity
                            })
                            .ToList();

                        foreach (var item in detailLists)
                        {
                            IUpdateTaskTransferRequisitionFinalizeDetail iUpdateTaskTransferRequisitionFinalizeDetail = new DUpdateTaskTransferRequisitionFinalizeDetail();
                            iUpdateTaskTransferRequisitionFinalizeDetail.UpdateTransferRequisitionDetailForOrderedQuantityDecrease((Guid)item.RequisitionId, item.ProductId, item.UnitTypeId, item.Quantity, item.ProductDimensionId);
                        }

                        // should set IsSettled = false into Task_TransferRequisitionFinalize table
                        var requisitionFinalizeIds = detailLists
                            .Select(s => s.RequisitionId)
                            .Distinct()
                            .ToList();
                        
                        foreach (var indId in requisitionFinalizeIds)
                        {
                            IUpdateTaskTransferRequisitionFinalize taskTransferRequisitionFinalize = new DUpdateTaskTransferRequisitionFinalize((Guid)indId);
                            taskTransferRequisitionFinalize.UpdateTransferRequisitionFinalizeForIsSettled(false);
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

        public CommonResult ApproveTransferOrder(Guid id, long companyId, long userId)
        {
            try
            {
                ISelectTaskTransferOrder iSelectTaskTransferOrder = new DSelectTaskTransferOrder(companyId);
                var selectedTransferOrder = iSelectTaskTransferOrder.SelectTaskTransferOrder(id)
                    .Where(x => x.OrderId == id);

                // Check finalize already exist or not
                if (selectedTransferOrder.Count() == 0)
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Selected Transfer Requisition Finalize not found."
                    };
                }

                // Check finalize already approved or not
                if (selectedTransferOrder.Where(x => x.Approved.Equals("A")).Count() > 0)
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Selected Transfer Order already approved."
                    };
                }

                // Check finalize already cancelled or not
                if (selectedTransferOrder.Where(x => x.Approved.Equals("C")).Count() > 0)
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Selected Transfer Order already cancelled."
                    };
                }

                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, ApplicationState.TransactionOptions))
                {
                    IUpdateTaskTransferOrder iUpdateTaskTransferOrder = new DUpdateTaskTransferOrder(id);
                    bool isSuccess = iUpdateTaskTransferOrder.UpdateTransferOrderForApprove(userId);
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