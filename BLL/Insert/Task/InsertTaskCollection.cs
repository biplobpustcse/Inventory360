using Inventory360DataModel;
using Inventory360DataModel.Task;
using BLL.Common;
using DAL.DataAccess.Delete.Task;
using DAL.DataAccess.Insert.Task;
using DAL.DataAccess.Select.Configuration;
using DAL.DataAccess.Select.Task;
using DAL.DataAccess.Update.Setup;
using DAL.DataAccess.Update.Task;
using DAL.Interface.Delete.Task;
using DAL.Interface.Insert.Task;
using DAL.Interface.Select.Configuration;
using DAL.Interface.Select.Task;
using DAL.Interface.Update.Setup;
using DAL.Interface.Update.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace BLL.Insert.Task
{
    public class InsertTaskCollection: GenerateDifferentEventPrefix
    {
        private string GenerateCollectionNo(DateTime date, long locationId, long companyId)
        {
            string generatedNo = string.Empty;
            string prefix = string.Empty;

            ISelectConfigurationOperationalEventDetail iSelectConfigurationOperationalEventDetail = new DSelectConfigurationOperationalEventDetail(companyId);
            var eventConfigInfo = iSelectConfigurationOperationalEventDetail.SelectOperationalEventDetailAll()
                .Where(x => x.Configuration_OperationalEvent.EventName.Equals(CommonEnum.OperationalEvent.Sales.ToString())
                    && x.Configuration_OperationalEvent.SubEventName.Equals(CommonEnum.OperationalSubEvent.Collection.ToString())
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

            // first lock the collectionnos table by temp data
            IInsertTaskCollectionNos iInsertTaskCollectionNos = new DInsertTaskCollectionNos(prefix + ("0".PadLeft(6, '0')), date.Year, companyId);
            iInsertTaskCollectionNos.InsertCollectionNos();

            // select last collection no from collectionnos table
            ISelectTaskCollectionNos iSelectTaskCollectionNos = new DSelectTaskCollectionNos(companyId);
            string previousCollectionNo = iSelectTaskCollectionNos.SelectCollectionNosAll()
                .Where(x => x.CollectionNo.ToLower().StartsWith(prefix.ToLower()))
                .OrderByDescending(o => o.CollectionNo)
                .Select(s => s.CollectionNo)
                .FirstOrDefault();

            // delete temp data from collectionnos table
            IDeleteTaskCollectionNos iDeleteTaskCollectionNos = new DDeleteTaskCollectionNos();
            iDeleteTaskCollectionNos.DeleteCollectionNos(prefix, date.Year, companyId);

            // if no record found, then start with 1
            // otherwise start with next value
            if (string.IsNullOrEmpty(previousCollectionNo))
            {
                generatedNo = prefix + ("1".PadLeft(6, '0'));
            }
            else
            {
                long currentValue = 0;
                long.TryParse(previousCollectionNo.Substring(previousCollectionNo.Length - 6), out currentValue);
                long nextValue = ++currentValue;
                generatedNo = prefix + (nextValue.ToString().PadLeft(6, '0'));
            }

            // insert new collection no to collectionnos table
            iInsertTaskCollectionNos = new DInsertTaskCollectionNos(generatedNo, date.Year, companyId);
            iInsertTaskCollectionNos.InsertCollectionNos();

            return generatedNo;
        }

        private CommonResult InsertSalesCollectionFinally(CommonTaskCollection entity)
        {
            ISelectConfigurationOperationType iSelectConfigurationOperationType = new DSelectConfigurationOperationType();
            ISelectConfigurationOperationalEvent iSelectConfigurationOperationalEvent = new DSelectConfigurationOperationalEvent();

            // get currency exchange rate
            var currencyInfo = GetCompanyCurrencyInfo.CompanyCurrencyInfo(entity.CompanyId);
            var currencyRate = GetCurrencyRateInfo.GetCurrencyRate(entity.CompanyId);

            // generate collection no
            string collectionNo = GenerateCollectionNo(entity.CollectionDate, entity.LocationId, entity.CompanyId);

            // save collection into Task_Collection table
            Guid collectionId = Guid.NewGuid();
            entity.CollectionId = collectionId;
            entity.CollectionNo = collectionNo;
            entity.OperationTypeId = iSelectConfigurationOperationType.SelectOperationTypeAll()
                .Where(x => x.Name == CommonEnum.OperationType.Regular.ToString())
                .Select(s => s.OperationTypeId)
                .FirstOrDefault();
            entity.OperationalEventId = iSelectConfigurationOperationalEvent.SelectOperationalEventAll()
                .Where(x => x.EventName == CommonEnum.OperationalEvent.Sales.ToString() && x.SubEventName == CommonEnum.OperationalSubEvent.Collection.ToString())
                .Select(s => s.OperationalEventId)
                .FirstOrDefault();

            CurrencyConvertedAmount collectedAmount = new CurrencyConvertedAmount();
            collectedAmount = GetCurrencyConversion.GetConvertedCurrencyAmount(currencyInfo, currencyRate, entity.CompanyId, entity.SelectedCurrency, entity.CollectedAmount, entity.ExchangeRate);

            IInsertTaskCollection iInsertTaskCollection = new DInsertTaskCollection(entity, collectedAmount);
            iInsertTaskCollection.InsertCollection();

            // save collection mapping into Task_CollectionMapping table
            foreach (CommonTaskCollectionMapping item in entity.CollectionMappingLists)
            {
                item.CollectionId = collectionId;
                collectedAmount = GetCurrencyConversion.GetConvertedCurrencyAmount(currencyInfo, currencyRate, entity.CompanyId, entity.SelectedCurrency, item.Amount, entity.ExchangeRate);

                IInsertTaskCollectionMapping iInsertTaskCollectionMapping = new DInsertTaskCollectionMapping(item, collectedAmount);
                iInsertTaskCollectionMapping.InsertCollectionMapping();

                // if mapping done by invoice/bill
                if (item.InvoiceId != null)
                {
                    IUpdateTaskSalesInvoice iUpdateTaskSalesInvoice = new DUpdateTaskSalesInvoice((Guid)item.InvoiceId);
                    // update invoice by collected amount
                    iUpdateTaskSalesInvoice.UpdateSalesInvoiceByCollectedAmountIncrease(collectedAmount);

                    // update invoice is settled or not
                    ISelectTaskSalesInvoice iSelectTaskSalesInvoice = new DSelectTaskSalesInvoice(entity.CompanyId);
                    bool isSettled = iSelectTaskSalesInvoice.CheckSalesInvoiceIsSettledByCollection((Guid)item.InvoiceId);
                    iUpdateTaskSalesInvoice.UpdateSalesInvoiceForIsSettled(isSettled);
                }
                // if mapping done by sales order
                else if (item.SalesOrderId != null)
                {
                    IUpdateTaskSalesOrder iUpdateTaskSalesOrder = new DUpdateTaskSalesOrder((Guid)item.SalesOrderId);
                    iUpdateTaskSalesOrder.UpdateSalesOrderByCollectionInCollectedAmountIncrease(collectedAmount);
                }
                // if mapping done by previous that is opening of customer
                else
                {
                    IUpdateSetupCustomer iUpdateSetupCustomer = new DUpdateSetupCustomer(entity.CustomerId);
                    iUpdateSetupCustomer.UpdateCustomerOpeningByCollectionInCollectedAmountAsIncrease(collectedAmount);
                }
            }

            ISelectConfigurationPaymentMode iSelectConfigurationPaymentMode = new DSelectConfigurationPaymentMode();
            var paymentModeList = iSelectConfigurationPaymentMode.SelectPaymentModeAll()
                .Select(s => new
                {
                    s.PaymentModeId,
                    s.NeedDetail
                })
                .ToList();

            // save collection detail into Task_CollectionDetail table
            var collectionDetail = entity.CollectionDetailLists.GroupBy(p => p.PaymentModeId,
                (key, g) => new
                {
                    PaymentModeId = key,
                    Amount = g.Sum(s => s.Amount)
                });

            foreach (var item in collectionDetail)
            {
                collectedAmount = GetCurrencyConversion.GetConvertedCurrencyAmount(currencyInfo, currencyRate, entity.CompanyId, entity.SelectedCurrency, item.Amount, entity.ExchangeRate);

                CommonTaskCollectionDetail newItem = new CommonTaskCollectionDetail();
                newItem.CollectionDetailId = Guid.NewGuid();
                newItem.CollectionId = collectionId;
                newItem.PaymentModeId = item.PaymentModeId;

                IInsertTaskCollectionDetail iInsertTaskCollectionDetail = new DInsertTaskCollectionDetail(newItem, collectedAmount);
                iInsertTaskCollectionDetail.InsertCollectionDetail();

                // cheque info save into Task_ChequeInfo table
                if (paymentModeList.Where(x => x.PaymentModeId == item.PaymentModeId).Select(s => s.NeedDetail).FirstOrDefault())
                {
                    foreach (var chequeItem in entity.CollectionDetailLists.Where(x => x.PaymentModeId == item.PaymentModeId).ToList())
                    {
                        CurrencyConvertedAmount chequeAmount = GetCurrencyConversion.GetConvertedCurrencyAmount(currencyInfo, currencyRate, entity.CompanyId, entity.SelectedCurrency, chequeItem.Amount, entity.ExchangeRate);

                        IInsertTaskChequeInfo iInsertTaskChequeInfo = new DInsertTaskChequeInfo(chequeItem.BankId, chequeItem.ChequeNo, chequeItem.ChequeDate, entity.LocationId, entity.CompanyId, entity.EntryBy, chequeAmount, newItem.CollectionDetailId, null, null);
                        iInsertTaskChequeInfo.InsertChequeInfo();
                    }
                }
            }

            return new CommonResult()
            {
                IsSuccess = true,
                Message = collectionNo,
                Message1 = collectionId.ToString()
            };
        }

        public CommonResult InsertSalesCollection(CommonTaskCollection entity)
        {
            try
            {
                CommonResult result = new CommonResult();

                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, ApplicationState.TransactionOptions))
                {
                    result = InsertSalesCollectionFinally(entity);

                    transaction.Complete();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CommonResult InsertSalesCollectionFromDirectSales(CommonTaskDirectSales entity, Guid invoiceId)
        {
            try
            {
                ISelectConfigurationOperationType iSelectConfigurationOperationType = new DSelectConfigurationOperationType();
                ISelectConfigurationOperationalEvent iSelectConfigurationOperationalEvent = new DSelectConfigurationOperationalEvent();

                CommonTaskCollection finalEntity = new CommonTaskCollection();
                finalEntity.CollectionDate = entity.InvoiceDate;
                finalEntity.SelectedCurrency = entity.SelectedCurrency;
                finalEntity.CollectedAmount = entity.Collection;
                finalEntity.CustomerId = entity.CustomerId;
                finalEntity.SalesPersonId = entity.SalesPersonId;
                finalEntity.CollectedBy = entity.EntryBy;
                finalEntity.OperationTypeId = iSelectConfigurationOperationType.SelectOperationTypeAll()
                                                    .Where(x => x.Name.Equals(CommonEnum.OperationType.Regular.ToString()))
                                                    .Select(s => s.OperationTypeId)
                                                    .FirstOrDefault();
                finalEntity.OperationalEventId = iSelectConfigurationOperationalEvent.SelectOperationalEventAll()
                                                    .Where(x => x.EventName == CommonEnum.OperationalEvent.Sales.ToString() && x.SubEventName == CommonEnum.OperationalSubEvent.Collection.ToString())
                                                    .Select(s => s.OperationalEventId)
                                                    .FirstOrDefault();
                finalEntity.LocationId = entity.LocationId;
                finalEntity.CompanyId = entity.CompanyId;
                finalEntity.EntryBy = entity.EntryBy;
                finalEntity.CollectionMappingLists = new List<CommonTaskCollectionMapping> { new CommonTaskCollectionMapping { InvoiceId = invoiceId, Amount = (entity.InvoiceAmount - entity.InvoiceDiscount) } };
                finalEntity.CollectionDetailLists = entity.CollectionInfoLists.Select(s => new CommonTaskCollectionDetail
                {
                    PaymentModeId = s.PaymentModeId,
                    Amount = s.Amount,
                    BankId = s.BankId,
                    ChequeNo = s.ChequeNo,
                    ChequeDate = s.ChequeDate
                }).ToList();

                return InsertSalesCollectionFinally(finalEntity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}