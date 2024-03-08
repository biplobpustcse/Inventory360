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
    public class InsertTaskReplacementClaim : GenerateDifferentEventPrefix
    {
        private string GenerateClaimNo(DateTime date, long locationId, long companyId)
        {
            string generatedNo = string.Empty;
            string prefix = string.Empty;

            ISelectConfigurationOperationalEventDetail iSelectConfigurationOperationalEventDetail = new DSelectConfigurationOperationalEventDetail(companyId);
            var eventConfigInfo = iSelectConfigurationOperationalEventDetail.SelectOperationalEventDetailAll()
                .Where(x => x.Configuration_OperationalEvent.EventName.Equals(CommonEnum.OperationalEvent.RMA.ToString())
                    && x.Configuration_OperationalEvent.SubEventName.Equals(CommonEnum.OperationalSubEvent.ReplacementClaim.ToString())
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
            IInsertTaskReplacementClaimNos iInsertTaskReplacementClaimNos = new DInsertTaskReplacementClaimNos(prefix + ("0".PadLeft(6, '0')), date.Year, companyId);
            iInsertTaskReplacementClaimNos.InsertReplacementClaimNos();

            // select last finalize no from requisiton finalize nos table
            ISelectTaskReplacementClaimNos iSelectTaskReplacementClaimNos = new DSelectTaskReplacementClaimNos(companyId);
            string previousReceiveNo = iSelectTaskReplacementClaimNos.SelectReplacementClaimNosAll()
                .Where(x => x.ClaimNo.ToLower().StartsWith(prefix.ToLower()))
                .OrderByDescending(o => o.ClaimNo)
                .Select(s => s.ClaimNo)
                .FirstOrDefault();

            // delete temp data from requisition finalize nos table
            IDeleteTaskReplacementClaimNos iDeleteTaskReplacementClaimNos = new DDeleteTaskReplacementClaimNos();
            iDeleteTaskReplacementClaimNos.DeleteReplacementClaimNos(prefix, date.Year, companyId);

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
            iInsertTaskReplacementClaimNos = new DInsertTaskReplacementClaimNos(generatedNo, date.Year, companyId);
            iInsertTaskReplacementClaimNos.InsertReplacementClaimNos();

            return generatedNo;
        }

        private CommonResult InsertReplacementClaimFinally(CommonReplacementClaim entity)
        {
            //generate receive no
            string claimNo = GenerateClaimNo((DateTime)MyConversion.ConvertDateStringToDate(entity.ClaimDate), entity.LocationId, entity.CompanyId);

            //save ReplacementClaim data into Task_ReplacementClaim table
            Guid claimId = Guid.NewGuid();
            entity.ClaimId = claimId;
            entity.ClaimNo = claimNo;

            IInsertTaskReplacementClaim iInsertTaskReplacementClaim = new DInsertTaskReplacementClaim(entity);
            iInsertTaskReplacementClaim.InsertReplacementClaim();

            // save Task_ReplacementClaimDetail
            foreach (CommonReplacementClaimDetail item in entity.replacementClaimDetail)
            {
                item.ClaimDetailId = Guid.NewGuid();
                item.ClaimId = claimId;

                IInsertTaskReplacementClaimDetail iInsertTaskReplacementClaimDetail = new DInsertTaskReplacementClaimDetail(item);
                iInsertTaskReplacementClaimDetail.InsertReplacementClaimDetail();

                //save problem data into Task_ReplacementClaimDetail_Problem table
                foreach (CommonReplacementClaimDetail_Problem probItem in item.replacementClaimDetail_Problem)
                {
                    probItem.ClaimDetailProblemId = Guid.NewGuid();
                    probItem.ClaimDetailId = item.ClaimDetailId;
                    IInsertTaskReplacementClaimDetail_Problem iInsertTaskReplacementClaimDetail_Problem = new DInsertTaskReplacementClaimDetail_Problem(probItem);
                    iInsertTaskReplacementClaimDetail_Problem.InsertReplacementClaimDetail_Problem();
                }

            }

            return new CommonResult()
            {
                IsSuccess = true,
                Message = claimNo
            };
        }

        public CommonResult InsertReplacementClaim(CommonReplacementClaim entity)
        {
            try
            {
                CommonResult result = new CommonResult();

                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, ApplicationState.TransactionOptions))
                {
                    result = InsertReplacementClaimFinally(entity);

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