using Inventory360DataModel;
using Inventory360DataModel.Task;
using BLL.Common;
using DAL.DataAccess.Delete.Task;
using DAL.DataAccess.Insert.Task;
using DAL.DataAccess.Select.Configuration;
using DAL.DataAccess.Select.Task;
using DAL.Interface.Delete.Task;
using DAL.Interface.Insert.Task;
using DAL.Interface.Select.Configuration;
using DAL.Interface.Select.Task;
using System;
using System.Linq;
using System.Transactions;

namespace BLL.Insert.Task
{
    public class InsertTaskReplacementReceive : GenerateDifferentEventPrefix
    {
        private string GenerateReplacementReceiveNo(DateTime date, long locationId, long companyId)
        {
            string generatedNo = string.Empty;
            string prefix = string.Empty;

            ISelectConfigurationOperationalEventDetail iSelectConfigurationOperationalEventDetail = new DSelectConfigurationOperationalEventDetail(companyId);
            var eventConfigInfo = iSelectConfigurationOperationalEventDetail.SelectOperationalEventDetailAll()
                .Where(x => x.Configuration_OperationalEvent.EventName.Equals(CommonEnum.OperationalEvent.RMA.ToString())
                    && x.Configuration_OperationalEvent.SubEventName.Equals(CommonEnum.OperationalSubEvent.ReplacementReceive.ToString())
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
            IInsertTaskReplacementReceiveNos iInsertTaskReplacementReceiveNos = new DInsertTaskReplacementReceiveNos(prefix + ("0".PadLeft(6, '0')), date.Year, companyId);
            iInsertTaskReplacementReceiveNos.InsertReplacementReceiveNos();

            // select last finalize no from requisiton finalize nos table
            ISelectTaskReplacementReceiveNos iSelectTaskReplacementReceiveNos = new DSelectTaskReplacementReceiveNos(companyId);
            string previousReceiveNo = iSelectTaskReplacementReceiveNos.SelectReplacementReceiveNosAll()
                .Where(x => x.ReceiveNo.ToLower().StartsWith(prefix.ToLower()))
                .OrderByDescending(o => o.ReceiveNo)
                .Select(s => s.ReceiveNo)
                .FirstOrDefault();

            // delete temp data from requisition finalize nos table
            IDeleteTaskReplacementReceiveNos iDeleteTaskReplacementReceiveNos = new DDeleteTaskReplacementReceiveNos();
            iDeleteTaskReplacementReceiveNos.DeleteReplacementReceiveNos(prefix, date.Year, companyId);

            // if no record found, then start with 1
            // otherwise start with next value
            if (string.IsNullOrEmpty(previousReceiveNo))
            {
                generatedNo = prefix + ("1".PadLeft(6, '0'));
            }
            else
            {
                long currentValue = 0;
                long.TryParse(previousReceiveNo.Substring(previousReceiveNo.Length - 6), out currentValue);
                long nextValue = ++currentValue;
                generatedNo = prefix + (nextValue.ToString().PadLeft(6, '0'));
            }

            // insert new finalize no to requisitionfinalizenos table
            iInsertTaskReplacementReceiveNos = new DInsertTaskReplacementReceiveNos(generatedNo, date.Year, companyId);
            iInsertTaskReplacementReceiveNos.InsertReplacementReceiveNos();

            return generatedNo;
        }

        private CommonResult InsertReplacementReceiveFinally(CommonTaskReplacementReceive entity)
        {
            //generate deliveryNo no
            string receiveNo = GenerateReplacementReceiveNo((DateTime)MyConversion.ConvertDateStringToDate(entity.ReceiveDate), entity.LocationId, entity.CompanyId);

            //save ReplacementReceive data into Task_ReplacementReceive table
            Guid receiveId = Guid.NewGuid();
            entity.ReceiveId = receiveId;
            entity.ReceiveNo = receiveNo;

            IInsertTaskReplacementReceive iInsertTaskReplacementReceive = new DInsertTaskReplacementReceive(entity);
            iInsertTaskReplacementReceive.InsertReplacementReceive();

            //save charge data into Task_ReplacementReceive_Charge table
            foreach (CommonTaskReplacementReceive_Charge chargeItem in entity.ReplacementReceiveCharge)
            {
                chargeItem.ReceiveChargeId = Guid.NewGuid();
                chargeItem.ReceiveId = entity.ReceiveId;

                ISelectConfigurationEventWiseCharge iSelectConfigurationEventWiseCharge = new DSelectConfigurationEventWiseCharge(entity.CompanyId);
                var ChargeEventId = iSelectConfigurationEventWiseCharge.SelectEventWiseChargeAll()
                    .Where(x => x.EventName.Equals(CommonEnum.OperationalEvent.RMA.ToString())
                        && x.ChargeId == chargeItem.ChargeId)
                    .Select(s => s.ChargeEventId)
                    .DefaultIfEmpty(0)
                    .FirstOrDefault();

                if (ChargeEventId == 0)
                {
                    throw new Exception("Event Wise Charge is not configured.");
                }
                chargeItem.ChargeEventId = ChargeEventId;

                IInsertTaskReplacementReceive_Charge iInsertTaskReplacementReceive_Charge = new DInsertTaskReplacementReceive_Charge(chargeItem);
                iInsertTaskReplacementReceive_Charge.InsertReplacementReceive_Charge();
            }
            // save CustomerDelivery detail
            foreach (CommonTaskReplacementReceiveDetail item in entity.ReplacementReceiveDetail)
            {
                item.ReceiveDetailId = Guid.NewGuid();
                item.ReceiveId = entity.ReceiveId;
                IInsertTaskReplacementReceiveDetail iInsertTaskReplacementReceiveDetail = new DInsertTaskReplacementReceiveDetail(item);
                iInsertTaskReplacementReceiveDetail.InsertReplacementReceiveDetail();
            }
            return new CommonResult()
            {
                IsSuccess = true,
                Message = receiveNo
            };
        }

        public CommonResult InsertReplacementReceive(CommonTaskReplacementReceive entity)
        {
            try
            {
                CommonResult result = new CommonResult();

                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, ApplicationState.TransactionOptions))
                {
                    result = InsertReplacementReceiveFinally(entity);

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