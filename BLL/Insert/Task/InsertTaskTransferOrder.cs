using Inventory360DataModel;
using Inventory360DataModel.Task;
using BLL.Common;
using DAL.DataAccess.Delete.Task;
using DAL.DataAccess.Insert.Task;
using DAL.DataAccess.Select.Configuration;
using DAL.DataAccess.Select.Task;
using DAL.DataAccess.Update.Task;
using DAL.Interface.Delete.Task;
using DAL.Interface.Insert.Task;
using DAL.Interface.Select.Configuration;
using DAL.Interface.Select.Task;
using DAL.Interface.Update.Task;
using System;
using System.Linq;
using System.Transactions;

namespace BLL.Insert.Task
{
    public class InsertTaskTransferOrder : GenerateDifferentEventPrefix
    {
        private string GenerateTransferOrderNo(DateTime date, long locationId, long companyId)
        {
            string generatedNo = string.Empty;
            string prefix = string.Empty;

            ISelectConfigurationOperationalEventDetail iSelectConfigurationOperationalEventDetail = new DSelectConfigurationOperationalEventDetail(companyId);
            var eventConfigInfo = iSelectConfigurationOperationalEventDetail.SelectOperationalEventDetailAll()
                .Where(x => x.Configuration_OperationalEvent.EventName.Equals(CommonEnum.OperationalEvent.Transfer.ToString())
                    && x.Configuration_OperationalEvent.SubEventName.Equals(CommonEnum.OperationalSubEvent.TransferOrder.ToString())
                    && x.LocationId == locationId
                    && x.CompanyId == companyId)
                .Select(s => new
                {
                    s.Prefix,
                    s.NumberFormat
                })
                .FirstOrDefault();

            if (eventConfigInfo == null)
            {
                throw new Exception("Operational Event is not configured.");
            }

            prefix = GenerateDifferentEventPrefixAsNo(eventConfigInfo.Prefix, eventConfigInfo.NumberFormat, date, companyId, locationId);

            // first lock the requisition finalize table by temp data
            IInsertTaskTransferOrderNos iInsertTaskTransferOrderNos = new DInsertTaskTransferOrderNos(prefix + ("0".PadLeft(6, '0')), date.Year, companyId);
            iInsertTaskTransferOrderNos.InsertTaskTransferOrderNos();

            // select last finalize no from requisiton finalize nos table
            ISelectTaskTransferOrderNos iSelectTaskTransferOrderNos = new DSelectTaskTransferOrderNos(companyId);
            string previousFinalizeNo = iSelectTaskTransferOrderNos.SelectTaskTransferOrderNosAll()
                .Where(x => x.OrderNo.ToLower().StartsWith(prefix.ToLower()))
                .OrderByDescending(o => o.OrderNo)
                .Select(s => s.OrderNo)
                .FirstOrDefault();

            // delete temp data from requisition finalize nos table
            IDeleteTaskTransferOrderNos iDeleteTaskTransferOrderNos = new DDeleteTaskTransferOrderNos();
            iDeleteTaskTransferOrderNos.DeleteTaskTransferOrderNos(prefix, date.Year, companyId);

            // if no record found, then start with 1
            // otherwise start with next value
            if (string.IsNullOrEmpty(previousFinalizeNo))
            {
                generatedNo = prefix + ("1".PadLeft(6, '0'));
            }
            else
            {
                long currentValue = 0;
                long.TryParse(previousFinalizeNo.Substring(previousFinalizeNo.Length - 6), out currentValue);
                long nextValue = ++currentValue;
                generatedNo = prefix + (nextValue.ToString().PadLeft(6, '0'));
            }

            // insert new finalize no to requisitionfinalizenos table
            iInsertTaskTransferOrderNos = new DInsertTaskTransferOrderNos(generatedNo, date.Year, companyId);
            iInsertTaskTransferOrderNos.InsertTaskTransferOrderNos();

            return generatedNo;
        }

        private CommonResult InsertTransferOrderFinally(CommonTransferOrder entity, long entryBy)
        {
            // generate transfer order no
            string orderNo = GenerateTransferOrderNo(entity.OrderDate, entity.LocationId, entity.CompanyId);

            // save finalize data into Task_TransferOrder table
            entity.OrderId = Guid.NewGuid();
            entity.OrderNo = orderNo;
            entity.EntryBy = entryBy;

            IInsertTaskTransferOrder iInsertTaskTransferOrder = new DInsertTaskTransferOrder(entity);
            iInsertTaskTransferOrder.InsertTaskTransferOrder();

            // save Transfer Order Detail
            foreach (CommonTransferOrderDetail item in entity.TransferOrderDetailList)
            {
                item.TransferOrderDetailId = Guid.NewGuid();
                item.TransferOrderId = entity.OrderId;

                IInsertTaskTransferOrderDetail iInsertTaskTransferOrderDetail = new DInsertTaskTransferOrderDetail(item);
                iInsertTaskTransferOrderDetail.InsertTaskTransferOrderDetail();

                #region should increase into Task_TransferRequisitionFinalizeDetail table
                if (item.RequisitionFinalizeId != null)
                {
                    IUpdateTaskTransferRequisitionFinalizeDetail iUpdateTaskTransferRequisitionFinalizeDetail = new DUpdateTaskTransferRequisitionFinalizeDetail();
                    iUpdateTaskTransferRequisitionFinalizeDetail.UpdateTransferRequisitionDetailForOrderedQuantityIncrease((Guid)item.RequisitionFinalizeId, item.ProductId, item.UnitTypeId, item.Quantity, item.ProductDimensionId);
                }
                #endregion
            }

            // check transfer requisition finalize fully complete or not
            // if complete then IsSettled = true
            var requisitionFinalizeIds = entity.TransferOrderDetailList
                .Where(x => x.RequisitionFinalizeId != null)
                .Select(s => s.RequisitionFinalizeId)
                .Distinct()
                .ToList();

            foreach (var item in requisitionFinalizeIds)
            {
                ISelectTaskTransferRequisitionFinalizeDetail iSelectTaskTransferRequisitionFinalizeDetail = new DSelectTaskTransferRequisitionFinalizeDetail(entity.CompanyId);
                decimal remainingQty = iSelectTaskTransferRequisitionFinalizeDetail.SelectRequisitionFinalizeDetailAll()
                    .Where(x => x.RequisitionId == item)
                    .Select(s => s.Quantity - s.OrderedQuantity)
                    .DefaultIfEmpty(0)
                    .Sum();

                if (remainingQty == 0)
                {
                    IUpdateTaskTransferRequisitionFinalize iUpdateTaskTransferRequisitionFinalize = new DUpdateTaskTransferRequisitionFinalize((Guid)item);
                    iUpdateTaskTransferRequisitionFinalize.UpdateTransferRequisitionFinalizeForIsSettled(true);
                }
            }

            return new CommonResult()
            {
                IsSuccess = true,
                Message = orderNo,
                Message1 = entity.OrderId.ToString(),
            };
        }

        public CommonResult InsertTransferOrder(CommonTransferOrder entity, long entryBy)
        {
            try
            {
                CommonResult result = new CommonResult();

                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, ApplicationState.TransactionOptions))
                {
                    result = InsertTransferOrderFinally(entity, entryBy);

                    transaction.Complete();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}