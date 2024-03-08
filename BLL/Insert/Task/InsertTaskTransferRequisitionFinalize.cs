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
using Inventory360DataModel;
using Inventory360DataModel.Task;
using System;
using System.Linq;
using System.Transactions;

namespace BLL.Insert.Task
{
    public class InsertTaskTransferRequisitionFinalize : GenerateDifferentEventPrefix
    {
        private string GenerateFinalizeNo(DateTime date, long locationId, long companyId)
        {
            string generatedNo = string.Empty;
            string prefix = string.Empty;

            ISelectConfigurationOperationalEventDetail iSelectConfigurationOperationalEventDetail = new DSelectConfigurationOperationalEventDetail(companyId);
            var eventConfigInfo = iSelectConfigurationOperationalEventDetail.SelectOperationalEventDetailAll()
                .Where(x => x.Configuration_OperationalEvent.EventName.Equals(CommonEnum.OperationalEvent.Transfer.ToString())
                    && x.Configuration_OperationalEvent.SubEventName.Equals(CommonEnum.OperationalSubEvent.RequisitionFinalize.ToString())
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
            IInsertTaskTransferRequisitionFinalizeNos iInsertTaskTransferRequisitionFinalizeNos = new DInsertTaskTransferRequisitionFinalizeNos(prefix + ("0".PadLeft(6, '0')), date.Year, companyId);
            iInsertTaskTransferRequisitionFinalizeNos.InsertTransferRequisitionFinalizeNos();

            // select last finalize no from requisiton finalize nos table
            ISelectTaskTransferRequisitionFinalizeNos iSelectTaskTransferRequisitionFinalizeNos = new DSelectTaskTransferRequisitionFinalizeNos(companyId);
            string previousFinalizeNo = iSelectTaskTransferRequisitionFinalizeNos.SelectTransferRequisitionFinalizeNosAll()
                .Where(x => x.RequisitionNo.ToLower().StartsWith(prefix.ToLower()))
                .OrderByDescending(o => o.RequisitionNo)
                .Select(s => s.RequisitionNo)
                .FirstOrDefault();

            // delete temp data from requisition finalize nos table
            IDeleteTaskTransferRequisitionFinalizeNos iDeleteStockTransferFinalizeNos = new DDeleteTaskTransferRequisitionFinalizeNos();
            iDeleteStockTransferFinalizeNos.DeleteTransferRequisitionFinalizeNos(prefix, date.Year, companyId);

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
            iInsertTaskTransferRequisitionFinalizeNos = new DInsertTaskTransferRequisitionFinalizeNos(generatedNo, date.Year, companyId);
            iInsertTaskTransferRequisitionFinalizeNos.InsertTransferRequisitionFinalizeNos();

            return generatedNo;
        }

        private CommonResult InsertTransferRequisitionFinalizeFinally(CommonTransferRequisitionFinalize entity)
        {
            // generate requisition no
            string requisitionNo = GenerateFinalizeNo(entity.RequisitionDate, entity.LocationId, entity.CompanyId);

            // save finalize data into Task_RequisitionFinalize table
            Guid requisitionId = Guid.NewGuid();
            entity.RequisitionId = requisitionId;
            entity.RequisitionNo = requisitionNo;

            IInsertTaskTransferRequisitionFinalize iInsertTaskTransferRequisitionFinalize = new DInsertStockTransferFinalize(entity);
            iInsertTaskTransferRequisitionFinalize.InsertTransferRequisitionFinalize();

            // save requisition detail
            foreach (CommonTransferRequisitionFinalizeDetail item in entity.FinalizeDetailLists)
            {
                item.RequisitionDetailId = Guid.NewGuid();
                item.RequisitionId = requisitionId;

                IInsertTaskTransferRequisitionFinalizeDetail iInsertTaskTransferRequisitionFinalizeDetail = new DInsertTaskTransferRequisitionFinalizeDetail(item);
                iInsertTaskTransferRequisitionFinalizeDetail.InsertTransferRequisitionFinalizeDetail();

                // If finalize done against item requisition
                // then update finalized quantity into Task_ItemRequisitionDetail table (+)
                if (item.ItemRequisitionId != null)
                {
                    IUpdateTaskItemRequisitionDetail iUpdateTaskItemRequisitionDetail = new DUpdateTaskItemRequisitionDetail();
                    iUpdateTaskItemRequisitionDetail.UpdateItemRequisitionDetailForFinalizedQuantityIncrease((Guid)item.ItemRequisitionId, item.ProductId, item.UnitTypeId, item.Quantity, item.ProductDimensionId);
                }
            }

            // check item requisition fully complete or not
            // if complete then IsSettled = true
            var itemRequisitionIds = entity.FinalizeDetailLists
                .Where(x => x.ItemRequisitionId != null)
                .Select(s => s.ItemRequisitionId)
                .Distinct()
                .ToList();

            foreach (var item in itemRequisitionIds)
            {
                ISelectTaskItemRequisitionDetail iSelectTaskItemRequisitionDetail = new DSelectTaskItemRequisitionDetail(entity.CompanyId);
                decimal remainingQty = iSelectTaskItemRequisitionDetail.SelectItemRequisitionDetailAll()
                    .Where(x => x.RequisitionId == item)
                    .Select(s => s.Quantity - s.FinalizedQuantity)
                    .DefaultIfEmpty(0)
                    .Sum();

                if (remainingQty == 0)
                {
                    IUpdateTaskItemRequisition iUpdateTaskItemRequisition = new DUpdateTaskItemRequisition((Guid)item);
                    iUpdateTaskItemRequisition.UpdateItemRequisitionForIsSettled(true);
                }
            }

            return new CommonResult()
            {
                IsSuccess = true,
                Message = requisitionNo
            };
        }

        public CommonResult InsertTransferRequisitionFinalize(CommonTransferRequisitionFinalize entity)
        {
            try
            {
                CommonResult result = new CommonResult();

                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, ApplicationState.TransactionOptions))
                {
                    result = InsertTransferRequisitionFinalizeFinally(entity);

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