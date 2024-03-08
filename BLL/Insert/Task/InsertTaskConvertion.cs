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
    public class InsertTaskConvertion : GenerateDifferentEventPrefix
    {
        private string GenerateConvertionNo(DateTime date, long locationId, long companyId)
        {
            string generatedNo = string.Empty;
            string prefix = string.Empty;

            ISelectConfigurationOperationalEventDetail iSelectConfigurationOperationalEventDetail = new DSelectConfigurationOperationalEventDetail(companyId);
            var eventConfigInfo = iSelectConfigurationOperationalEventDetail.SelectOperationalEventDetailAll()
                .Where(x => x.Configuration_OperationalEvent.EventName.Equals(CommonEnum.OperationalEvent.Production.ToString())
                    && x.Configuration_OperationalEvent.SubEventName.Equals(CommonEnum.OperationalSubEvent.Convertion.ToString())
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

            // first lock the Task_ConvertionNos table by temp data
            IInsertTaskConvertionNos iInsertTaskConvertionNos = new DInsertTaskConvertionNos(prefix + ("0".PadLeft(6, '0')), date.Year, companyId);
            iInsertTaskConvertionNos.InsertConvertionNos();

            // select last ConvertionNo from Task_ConvertionNos table
            ISelectTaskConvertionNos iSelectTaskConvertionNos = new DSelectTaskConvertionNos(companyId);
            string previousConvertionNo = iSelectTaskConvertionNos.SelectConvertionNosAll()
                .Where(x => x.ConvertionNo.ToLower().StartsWith(prefix.ToLower()))
                .OrderByDescending(o => o.ConvertionNo)
                .Select(s => s.ConvertionNo)
                .FirstOrDefault();

            // delete temp data from Task_ConvertionNos nos table
            IDeleteTaskConvertionNos iDeleteTaskConvertionNos = new DDeleteTaskConvertionNos();
            iDeleteTaskConvertionNos.DeleteConvertionNos(prefix, date.Year, companyId);

            // if no record found, then start with 1
            // otherwise start with next value
            if (string.IsNullOrEmpty(previousConvertionNo))
            {
                generatedNo = prefix + ("1".PadLeft(6, '0'));
            }
            else
            {
                long currentValue = 0;
                long.TryParse(previousConvertionNo.Substring(previousConvertionNo.Length - 6), out currentValue);
                long nextValue = ++currentValue;
                generatedNo = prefix + (nextValue.ToString().PadLeft(6, '0'));
            }

            // insert new ConvertionNo no to Task_ConvertionNos table
            iInsertTaskConvertionNos = new DInsertTaskConvertionNos(generatedNo, date.Year, companyId);
            iInsertTaskConvertionNos.InsertConvertionNos();

            return generatedNo;
        }

        private CommonResult InsertConvertionFinally(CommonTaskConvertion entity)
        {
            //generate ConvertionNo no
            string convertionNo = GenerateConvertionNo(entity.ConvertionDate, entity.LocationId, entity.CompanyId);

            //save Convertion data into Task_Convertion table
            Guid convertionId = Guid.NewGuid();
            entity.ConvertionId = convertionId;
            entity.ConvertionNo = convertionNo;

            IInsertTaskConvertion iInsertTaskConvertion = new DInsertTaskConvertion(entity);
            iInsertTaskConvertion.InsertConvertion();
            // save ConvertionDetail 
            foreach (CommonTaskConvertionDetail item in entity.CommonTaskConvertionDetail)
            {
                item.ConvertionDetailId = Guid.NewGuid();
                item.ConvertionId = entity.ConvertionId;
                IInsertTaskConvertionDetail iInsertTaskConvertionDetail = new DInsertTaskConvertionDetail(item);
                iInsertTaskConvertionDetail.InsertConvertionDetail();
                // save ConvertionDetailSerial 
                foreach (CommonTaskConvertionDetailSerial itemSerial in item.CommonTaskConvertionDetailSerial)
                {
                    itemSerial.ConvertionDetailSerialId = Guid.NewGuid();
                    itemSerial.ConvertionDetailId = item.ConvertionDetailId;
                    IInsertTaskConvertionDetailSerial iInsertTaskConvertionDetailSerial = new DInsertTaskConvertionDetailSerial(itemSerial);
                    iInsertTaskConvertionDetailSerial.InsertConvertionDetailSerial();
                }
            }
            return new CommonResult()
            {
                IsSuccess = true,
                Message = convertionNo
            };
        }

        public CommonResult InsertConvertion(CommonTaskConvertion entity)
        {
            try
            {
                CommonResult result = new CommonResult();

                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, ApplicationState.TransactionOptions))
                {
                    result = InsertConvertionFinally(entity);

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