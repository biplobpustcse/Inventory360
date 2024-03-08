using Inventory360DataModel;
using DAL.DataAccess.Insert.Task;
using DAL.DataAccess.Select.Task;
using DAL.DataAccess.Update.Task;
using DAL.Interface.Insert.Task;
using DAL.Interface.Select.Task;
using DAL.Interface.Update.Task;
using System;
using System.Linq;

namespace BLL.Common
{
    public static class GenerateVoucherPosting
    {
        public static bool SavePostedVoucher(Guid voucherId, string voucherType, long userId, long companyId, long locationId)
        {
            try
            {
                // Update voucher as posted
                IUpdateTaskVoucher iUpdateTaskVoucher = new DUpdateTaskVoucher(voucherId);
                bool isSuccess = iUpdateTaskVoucher.UpdateVoucherForPosting(userId);

                if (isSuccess)
                {
                    // Get all detail items from voucher detail
                    ISelectTaskVoucherDetail iSelectTaskVoucherDetail = new DSelectTaskVoucherDetail(voucherId);
                    var detailLists = iSelectTaskVoucherDetail.SelectVoucherDetailByVoucherId()
                        .Select(s => new
                        {
                            VoucherDetailId = s.VoucherDetailId,
                            VoucherDate = s.Task_Voucher.Date,
                            AccountsId = s.AccountsId,
                            BalanceType = s.Setup_Accounts.BalanceType,
                            ProjectId = s.ProjectId,
                            Particulars = s.Particulars,
                            Debit = s.Debit,
                            Credit = s.Credit,
                            Currency1Rate = s.Currency1Rate,
                            Currency1Debit = s.Currency1Debit,
                            Currency1Credit = s.Currency1Credit,
                            Currency2Rate = s.Currency2Rate,
                            Currency2Debit = s.Currency2Debit,
                            Currency2Credit = s.Currency2Credit
                        })
                        .ToList();

                    // Save to posted voucher table
                    foreach (var item in detailLists)
                    {
                        decimal amount = 0;
                        decimal amount1 = 0;
                        decimal amount2 = 0;
                        if (item.BalanceType.Equals(CommonEnum.BalanceType.Dr.ToString()) && item.Debit != 0 && item.Credit == 0)
                        {
                            amount = item.Debit;
                            amount1 = item.Currency1Debit;
                            amount2 = item.Currency2Debit;
                        }
                        else if (item.BalanceType.Equals(CommonEnum.BalanceType.Dr.ToString()) && item.Debit == 0 && item.Credit != 0)
                        {
                            amount = item.Credit * -1;
                            amount1 = item.Currency1Credit * -1;
                            amount2 = item.Currency2Credit * -1;
                        }
                        else if (item.BalanceType.Equals(CommonEnum.BalanceType.Cr.ToString()) && item.Credit != 0 && item.Debit == 0)
                        {
                            amount = item.Credit;
                            amount1 = item.Currency1Credit;
                            amount2 = item.Currency2Credit;
                        }
                        else if (item.BalanceType.Equals(CommonEnum.BalanceType.Cr.ToString()) && item.Credit == 0 && item.Debit != 0)
                        {
                            amount = item.Debit * -1;
                            amount1 = item.Currency1Debit * -1;
                            amount2 = item.Currency2Debit * -1;
                        }

                        // Save
                        IInsertTaskPostedVoucher iInsertTaskPostedVoucher = new DInsertTaskPostedVoucher(new CommonTaskPostedVoucher
                        {
                            VoucherDetailId = item.VoucherDetailId,
                            VoucherDate = item.VoucherDate,
                            VoucherType = voucherType,
                            AccountsId = item.AccountsId,
                            ProjectId = item.ProjectId,
                            Amount = amount,
                            Rate1 = item.Currency1Rate,
                            Amount1 = amount1,
                            Rate2 = item.Currency2Rate,
                            Amount2 = amount2,
                            LocationId = locationId,
                            CompanyId = companyId,
                            EntryBy = userId
                        });

                        iInsertTaskPostedVoucher.InsertPostedVoucher();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}