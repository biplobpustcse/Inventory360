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
    public class InsertTaskComplainReceive : GenerateDifferentEventPrefix
    {
        private string GenerateReceiveNo(DateTime date, long locationId, long companyId)
        {
            string generatedNo = string.Empty;
            string prefix = string.Empty;

            ISelectConfigurationOperationalEventDetail iSelectConfigurationOperationalEventDetail = new DSelectConfigurationOperationalEventDetail(companyId);
            var eventConfigInfo = iSelectConfigurationOperationalEventDetail.SelectOperationalEventDetailAll()
                .Where(x => x.Configuration_OperationalEvent.EventName.Equals(CommonEnum.OperationalEvent.RMA.ToString())
                    && x.Configuration_OperationalEvent.SubEventName.Equals(CommonEnum.OperationalSubEvent.ComplainReceive.ToString())
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
            IInsertTaskComplainReceiveNos iInsertTaskComplainReceiveNos = new DInsertTaskComplainReceiveNos(prefix + ("0".PadLeft(6, '0')), date.Year, companyId);
            iInsertTaskComplainReceiveNos.InsertComplainReceiveNos();

            // select last finalize no from requisiton finalize nos table
            ISelectTaskComplainReceiveNos iSelectTaskComplainReceiveNos = new DSelectTaskComplainReceiveNos(companyId);
            string previousReceiveNo = iSelectTaskComplainReceiveNos.SelectComplainReceiveNosAll()
                .Where(x => x.ReceiveNo.ToLower().StartsWith(prefix.ToLower()))
                .OrderByDescending(o => o.ReceiveNo)
                .Select(s => s.ReceiveNo)
                .FirstOrDefault();

            // delete temp data from requisition finalize nos table
            IDeleteTaskComplainReceiveNos iDeleteTaskComplainReceiveNos = new DDeleteTaskComplainReceiveNos();
            iDeleteTaskComplainReceiveNos.DeleteComplainReceiveNos(prefix, date.Year, companyId);

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
            iInsertTaskComplainReceiveNos = new DInsertTaskComplainReceiveNos(generatedNo, date.Year, companyId);
            iInsertTaskComplainReceiveNos.InsertComplainReceiveNos();

            return generatedNo;
        }

        private CommonResult InsertComplainReceiveFinally(CommonComplainReceive entity)
        {
            //generate receive no
            string receiveNo = GenerateReceiveNo(entity.ReceiveDate, entity.LocationId, entity.CompanyId);

            // get currency exchange rate
            var currencyInfo = GetCompanyCurrencyInfo.CompanyCurrencyInfo(entity.CompanyId);
            var currencyRate = GetCurrencyRateInfo.GetCurrencyRate(entity.CompanyId);
            CurrencyConvertedAmount totalChargeAmount = new CurrencyConvertedAmount();
            totalChargeAmount = GetCurrencyConversion.GetConvertedCurrencyAmount(currencyInfo, currencyRate, entity.CompanyId, entity.SelectedCurrency, entity.TotalChargeAmount, entity.ExchangeRate);

            //save ComplainReceive data into Task_ComplainReceive table
            Guid receiveId = Guid.NewGuid();
            entity.ReceiveId = receiveId;
            entity.ReceiveNo = receiveNo;

            IInsertTaskComplainReceive iInsertTaskComplainReceive = new DInsertTaskComplainReceive(entity, totalChargeAmount);
            iInsertTaskComplainReceive.InsertComplainReceive();

            //save charge data into Task_ComplainReceive_Charge table
            foreach (CommonComplainReceive_Charge chargeItem in entity.ComplainReceive_Charge)
            {
                chargeItem.ReceiveChargeId = Guid.NewGuid();
                chargeItem.ReceiveId = entity.ReceiveId;

                IInsertTaskComplainReceive_Charge iInsertTaskComplainReceiveDetail_Charge = new DInsertTaskComplainReceive_Charge(chargeItem);
                iInsertTaskComplainReceiveDetail_Charge.InsertComplainReceiveDetail_Charge();
            }

            // save complain receive detail
            foreach (CommonComplainReceiveDetail item in entity.ComplainReceiveDetail)
            {
                item.ReceiveDetailId = Guid.NewGuid();
                item.ReceiveId = receiveId;

                IInsertTaskComplainReceiveDetail iInsertTaskComplainReceiveDetail = new DInsertTaskComplainReceiveDetail(item);
                iInsertTaskComplainReceiveDetail.InsertComplainReceiveDetail();

                //save problem data into Task_ComplainReceiveDetail_Problem table
                foreach (CommonComplainReceiveDetail_Problem probItem in item.ComplainReceiveDetail_Problem)
                {
                    probItem.ReceiveDetailProblemId = Guid.NewGuid();
                    probItem.ReceiveDetailId = item.ReceiveDetailId;

                    IInsertTaskComplainReceiveDetail_Problem iInsertTaskComplainReceiveDetail_Problem = new DInsertTaskComplainReceiveDetail_Problem(probItem);
                    iInsertTaskComplainReceiveDetail_Problem.InsertComplainReceiveDetail_Problem();
                }

                //save SpareProduct data into Task_ComplainReceiveDetail_SpareProduct table
                foreach (CommonComplainReceiveDetail_SpareProduct spareProductItem in item.ComplainReceiveDetail_SpareProduct)
                {
                    spareProductItem.ReceiveDetailSpareId = Guid.NewGuid();
                    spareProductItem.ReceiveDetailId = item.ReceiveDetailId;
                    if (spareProductItem.ProductDimensionId == 0)
                        spareProductItem.ProductDimensionId = null;

                    IInsertTaskComplainReceiveDetail_SpareProduct iInsertTaskComplainReceiveDetail_SpareProduct = new DInsertTaskComplainReceiveDetail_SpareProduct(spareProductItem);
                    iInsertTaskComplainReceiveDetail_SpareProduct.InsertComplainReceiveDetail_SpareProduct();
                }

            }
            return new CommonResult()
            {
                IsSuccess = true,
                Message = receiveNo,
                Message1 = receiveId.ToString()
            };
        }

        public CommonResult InsertComplainReceive(CommonComplainReceive entity)
        {
            try
            {
                CommonResult result = new CommonResult();

                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, ApplicationState.TransactionOptions))
                {
                    result = InsertComplainReceiveFinally(entity);

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