using Inventory360DataModel;
using DAL.DataAccess;
using DAL.DataAccess.Delete.Task;
using DAL.DataAccess.Insert.Task;
using DAL.DataAccess.Select.Configuration;
using DAL.DataAccess.Select.Setup;
using DAL.DataAccess.Select.Task;
using DAL.DataAccess.Update.Task;
using DAL.Interface;
using DAL.Interface.Delete.Task;
using DAL.Interface.Insert.Task;
using DAL.Interface.Select.Configuration;
using DAL.Interface.Select.Setup;
using DAL.Interface.Select.Task;
using DAL.Interface.Update.Task;
using System;
using System.Linq;

namespace BLL.Common
{
    public class GenerateAutoVoucher : GenerateDifferentEventPrefix
    {
        public bool GenerateAutoVoucherForEvent(string selectedCurrency, string eventName, string subEventName, string operationType, long paymentModeId, DateTime date, decimal amount, decimal exchangeRate1, decimal amount1, decimal exchangeRate2, decimal amount2, long locationId, long companyId, long entryBy, long customerOrBuyerOrSupplierOrProductId, out Guid voucherId)
        {
            try
            {
                voucherId = Guid.Empty;

                ISelectConfigurationOperationalEventDetail iSelectConfigurationOperationalEventDetail = new DSelectConfigurationOperationalEventDetail(companyId);
                var voucherConfigInfo = iSelectConfigurationOperationalEventDetail.SelectOperationalEventDetailAll()
                    .Where(x => x.Configuration_OperationalEvent.EventName.Equals(eventName)
                        && x.Configuration_OperationalEvent.SubEventName.Equals(subEventName)
                        && x.Configuration_OperationType.Name.Equals(operationType)
                        && x.Configuration_Voucher.CompanyId == companyId)
                    .WhereIf(paymentModeId != 0, x => x.PaymentModeId == paymentModeId)
                    .Select(s => new
                    {
                        s.Configuration_Voucher.Prefix,
                        s.Configuration_Voucher.NumberFormat,
                        s.Configuration_Voucher.Description,
                        Detail = s.Configuration_Voucher.Configuration_VoucherDetail.Select(sd => new
                        {
                            sd.Particulars,
                            sd.AccountsId,
                            sd.DrOrCr,
                            sd.AccountsSource
                        })
                        .ToList()
                    })
                    .FirstOrDefault();

                // If voucher config not found then voucher will not create
                if (voucherConfigInfo == null)
                {
                    return false;
                }

                string generatedNo = string.Empty;
                string numberPrefix = GenerateDifferentEventPrefixAsNo(voucherConfigInfo.Prefix, voucherConfigInfo.NumberFormat, date, companyId, locationId);

                // first lock the vouchernos table by temp data
                IInsertTaskVoucherNos iInsertTaskVoucherNos = new DInsertTaskVoucherNos(numberPrefix + ("0".PadLeft(6, '0')), date.Year, companyId);
                iInsertTaskVoucherNos.InsertVoucherNos();

                // select last voucher no from vouchernos table
                ISelectTaskVoucherNos iSelectTaskVoucherNos = new DSelectTaskVoucherNos(companyId);
                string previousVoucherNo = iSelectTaskVoucherNos.SelectVoucherNosAll()
                    .Where(x => x.VoucherNo.ToLower().StartsWith(numberPrefix.ToLower()))
                    .OrderByDescending(o => o.VoucherNo)
                    .Select(s => s.VoucherNo)
                    .FirstOrDefault();

                // delete temp data from vouchernos table
                IDeleteTaskVoucherNos iDeleteTaskVoucherNos = new DDeleteTaskVoucherNos();
                iDeleteTaskVoucherNos.DeleteVoucherNos(numberPrefix, date.Year, companyId);

                // if no record found, then start with 1
                // otherwise start with next value
                if (string.IsNullOrEmpty(previousVoucherNo))
                {
                    generatedNo = numberPrefix + ("1".PadLeft(6, '0'));
                }
                else
                {
                    long currentValue = 0;
                    long.TryParse(previousVoucherNo.Substring(previousVoucherNo.Length - 6), out currentValue);
                    long nextValue = ++currentValue;
                    generatedNo = numberPrefix + (nextValue.ToString().PadLeft(6, '0'));
                }

                // insert new voucher no to vouchernos table
                iInsertTaskVoucherNos = new DInsertTaskVoucherNos(generatedNo, date.Year, companyId);
                iInsertTaskVoucherNos.InsertVoucherNos();

                CommonTaskVoucher entity = new CommonTaskVoucher();

                // save voucher
                voucherId = Guid.NewGuid();
                entity.VoucherId = voucherId;
                entity.VoucherNo = generatedNo;
                entity.VoucherType = "Auto";
                entity.Date = date;
                entity.Description = voucherConfigInfo.Description;
                entity.LocationId = locationId;
                entity.CompanyId = companyId;
                entity.EntryBy = entryBy;
                entity.Currency = selectedCurrency;

                IInsertTaskVoucher iInsertTaskVoucher = new DInsertTaskVoucher(entity);
                iInsertTaskVoucher.InsertVoucher();

                // select project
                ISelectSetupProject iSelectSetupProject = new DSelectSetupProject(companyId);
                long projectId = iSelectSetupProject.SelectProjectAll()
                    .Select(s => s.ProjectId)
                    .FirstOrDefault();

                // save voucher detail
                foreach (var item in voucherConfigInfo.Detail)
                {
                    CurrencyConvertedVoucherAmount amountItem = new CurrencyConvertedVoucherAmount();
                    CommonTaskVoucherDetail entityDetail = new CommonTaskVoucherDetail();

                    // check accountsid or datasource
                    if (item.AccountsId != null)
                    {
                        entityDetail.AccountsId = (long)item.AccountsId;
                    }
                    else
                    {
                        try
                        {
                            IExecuteSQLQuery iExecuteSQLQuery = new DExecuteSQLQuery();
                            var value = iExecuteSQLQuery.ExecuteQuery(item.AccountsSource + customerOrBuyerOrSupplierOrProductId);

                            entityDetail.AccountsId = string.IsNullOrEmpty(value) ? 0 : Convert.ToInt64(value);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Accounts head is not assigned.");
                        }
                    }

                    // check Dr or Cr
                    if (item.DrOrCr == CommonEnum.BalanceType.Dr.ToString())
                    {
                        amountItem.BaseCurrencyDebit = amount;
                        amountItem.Currency1Debit = amount1;
                        amountItem.Currency2Debit = amount2;
                    }
                    else
                    {
                        amountItem.BaseCurrencyCredit = amount;
                        amountItem.Currency1Credit = amount1;
                        amountItem.Currency2Credit = amount2;
                    }

                    amountItem.Currency1Rate = exchangeRate1;
                    amountItem.Currency2Rate = exchangeRate2;
                    entityDetail.ProjectId = projectId;
                    entityDetail.Particulars = item.Particulars;

                    IInsertTaskVoucherDetail iInsertTaskVoucherDetail = new DInsertTaskVoucherDetail(voucherId, entityDetail, amountItem);
                    iInsertTaskVoucherDetail.InsertVoucherDetail();
                }

                // update voucher as approved
                IUpdateTaskVoucher iUpdateTaskVoucher = new DUpdateTaskVoucher(voucherId);
                iUpdateTaskVoucher.UpdateVoucherForApprove(entryBy);

                // save posted voucher
                GenerateVoucherPosting.SavePostedVoucher(voucherId, "Auto", entryBy, companyId, locationId);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}