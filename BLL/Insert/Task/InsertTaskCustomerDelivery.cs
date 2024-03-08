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
    public class InsertTaskCustomerDelivery : GenerateDifferentEventPrefix
    {
        private string GenerateCustomerDeliveryNo(DateTime date, long locationId, long companyId)
        {
            string generatedNo = string.Empty;
            string prefix = string.Empty;

            ISelectConfigurationOperationalEventDetail iSelectConfigurationOperationalEventDetail = new DSelectConfigurationOperationalEventDetail(companyId);
            var eventConfigInfo = iSelectConfigurationOperationalEventDetail.SelectOperationalEventDetailAll()
                .Where(x => x.Configuration_OperationalEvent.EventName.Equals(CommonEnum.OperationalEvent.RMA.ToString())
                    && x.Configuration_OperationalEvent.SubEventName.Equals(CommonEnum.OperationalSubEvent.CustomerDelivery.ToString())
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
            IInsertTaskCustomerDeliveryNos iInsertTaskCustomerDeliveryNos = new DInsertTaskCustomerDeliveryNos(prefix + ("0".PadLeft(6, '0')), date.Year, companyId);
            iInsertTaskCustomerDeliveryNos.InsertCustomerDeliveryNos();

            // select last finalize no from requisiton finalize nos table
            ISelectTaskCustomerDeliveryNos iSelectTaskCustomerDeliveryNos = new DSelectTaskCustomerDeliveryNos(companyId);
            string previousDeliveryNo = iSelectTaskCustomerDeliveryNos.SelectCustomerDeliveryNosAll()
                .Where(x => x.DeliveryNo.ToLower().StartsWith(prefix.ToLower()))
                .OrderByDescending(o => o.DeliveryNo)
                .Select(s => s.DeliveryNo)
                .FirstOrDefault();

            // delete temp data from requisition finalize nos table
            IDeleteTaskCustomerDeliveryNos iDeleteTaskCustomerDeliveryNos = new DDeleteTaskCustomerDeliveryNos();
            iDeleteTaskCustomerDeliveryNos.DeleteCustomerDeliveryNos(prefix, date.Year, companyId);

            // if no record found, then start with 1
            // otherwise start with next value
            if (string.IsNullOrEmpty(previousDeliveryNo))
            {
                generatedNo = prefix + ("1".PadLeft(6, '0'));
            }
            else
            {
                long currentValue = 0;
                long.TryParse(previousDeliveryNo.Substring(previousDeliveryNo.Length - 6), out currentValue);
                long nextValue = ++currentValue;
                generatedNo = prefix + (nextValue.ToString().PadLeft(6, '0'));
            }

            // insert new finalize no to requisitionfinalizenos table
            iInsertTaskCustomerDeliveryNos = new DInsertTaskCustomerDeliveryNos(generatedNo, date.Year, companyId);
            iInsertTaskCustomerDeliveryNos.InsertCustomerDeliveryNos();

            return generatedNo;
        }

        private CommonResult InsertCustomerDeliveryFinally(CommonTaskCustomerDelivery entity)
        {
            //generate deliveryNo no
            string deliveryNo = GenerateCustomerDeliveryNo((DateTime)MyConversion.ConvertDateStringToDate(entity.DeliveryDate), entity.LocationId, entity.CompanyId);

            //save ComplainReceive data into Task_CustomerDelivery table
            Guid deliveryId = Guid.NewGuid();
            entity.DeliveryId = deliveryId;
            entity.DeliveryNo = deliveryNo;

            IInsertTaskCustomerDelivery iInsertTaskCustomerDelivery = new DInsertTaskCustomerDelivery(entity);
            iInsertTaskCustomerDelivery.InsertCustomerDelivery();

            //save charge data into Task_CustomerDelivery_Charge table
            foreach (CommonTaskCustomerDelivery_Charge chargeItem in entity.CustomerDeliveryCharge)
            {
                chargeItem.DeliveryChargeId = Guid.NewGuid();
                chargeItem.DeliveryId = entity.DeliveryId;

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

                IInsertTaskCustomerDelivery_Charge iInsertTaskCustomerDelivery_Charge = new DInsertTaskCustomerDelivery_Charge(chargeItem);
                iInsertTaskCustomerDelivery_Charge.InsertCustomerDelivery_Charge();
            }

            // save CustomerDelivery detail
            foreach (CommonTaskCustomerDeliveryDetail item in entity.CustomerDeliveryDetail)
            {
                item.DeliveryDetailId = Guid.NewGuid();
                item.DeliveryId = entity.DeliveryId;

                IInsertTaskCustomerDeliveryDetail iInsertTaskCustomerDeliveryDetail = new DInsertTaskCustomerDeliveryDetail(item);
                iInsertTaskCustomerDeliveryDetail.InsertCustomerDeliveryDetail();

                //save problem data into Task_CustomerDeliveryDetail_Problem table
                foreach (CommonTaskCustomerDeliveryDetail_Problem probItem in item.customerDeliveryDetail_Problem)
                {
                    probItem.DeliveryDetailProblemId = Guid.NewGuid();
                    probItem.DeliveryDetailId = item.DeliveryDetailId;
                    IInsertTaskCustomerDeliveryDetail_Problem iInsertTaskCustomerDeliveryDetail_Problem = new DInsertTaskCustomerDeliveryDetail_Problem(probItem);
                    iInsertTaskCustomerDeliveryDetail_Problem.InsertCustomerDeliveryDetail_Problem();
                }

                //save SpareProduct data into Task_CustomerDeliveryDetail_SpareProduct table
                foreach (CommonTaskCustomerDeliveryDetail_SpareProduct spareProductItem in item.customerDeliveryDetail_SpareProduct)
                {
                    spareProductItem.DeliveryDetailSpareId = Guid.NewGuid();
                    spareProductItem.DeliveryDetailId = item.DeliveryDetailId;
                    if (spareProductItem.ProductDimensionId == 0)
                        spareProductItem.ProductDimensionId = null;

                    IInsertTaskCustomerDeliveryDetail_SpareProduct iInsertTaskCustomerDeliveryDetail_SpareProduct = new DInsertTaskCustomerDeliveryDetail_SpareProduct(spareProductItem);
                    iInsertTaskCustomerDeliveryDetail_SpareProduct.InsertCustomerDeliveryDetail_SpareProduct();
                }

            }
            return new CommonResult()
            {
                IsSuccess = true,
                Message = deliveryNo
            };
        }

        public CommonResult InsertCustomerDelivery(CommonTaskCustomerDelivery entity)
        {
            try
            {
                CommonResult result = new CommonResult();

                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, ApplicationState.TransactionOptions))
                {
                    result = InsertCustomerDeliveryFinally(entity);

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