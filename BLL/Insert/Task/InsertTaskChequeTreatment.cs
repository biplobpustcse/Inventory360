using Inventory360DataModel;
using Inventory360DataModel.Task;
using BLL.Common;
using DAL.DataAccess.Insert.Task;
using DAL.DataAccess.Update.Task;
using DAL.Interface.Insert.Task;
using DAL.Interface.Update.Task;
using System;
using System.Transactions;

namespace BLL.Insert.Task
{
    public class InsertTaskChequeTreatment : GenerateDifferentEventPrefix
    {       
        public CommonResult InsertChequeTreatment(CommonTaskChequeTreatment entity, long UserId)
        {
            try
            {
                CommonResult result = new CommonResult();

                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, ApplicationState.TransactionOptions))
                {
                    foreach (CommonTaskChequeTreatment item in entity.CommonTaskChequeTreatmentLists)
                    {                        

                        if (item.PreviousStatus == "S" && (item.Status != "D" && item.Status != "H"))
                        {
                            result.IsSuccess = false;
                            result.Message = "Sending Status must be Hohor/Dishonor!!!";
                            return result;
                        }
                        else if (item.PreviousStatus == "D" && (item.Status != "S" && item.Status != "B"))
                        {
                            result.IsSuccess = false;
                            result.Message = "Sending Status must be Send / Balance Adjusted!!!";
                            return result;
                        }
                        else if (item.PreviousStatus == "H" || item.PreviousStatus == "B")
                        {
                            result.IsSuccess = false;
                            result.Message = "Honor and Balance Adjustment cheque not allow for treatment!!!";
                            return result;
                        }
                        else if (item.PreviousStatus == null || item.PreviousStatus == "N" && item.Status != "S")
                        {
                            result.IsSuccess = false;
                            result.Message = "Sending Status must be Send!!!";
                            return result;
                        }
                        if(item.PreviousStatus !="N")
                        {
                            item.TreatmentBankId = item.PreviousTreatmentBankId;
                        }
                        item.TreatmentId = Guid.NewGuid();
                        if (item.VoucherId == Guid.Empty)
                            item.VoucherId = null;
                        item.EntryBy = UserId;
                        item.EntryDate = DateTime.Now;

                        IInsertTaskChequeTreatment iInsertTaskChequeTreatment = new DInsertTaskChequeTreatment(item);
                        iInsertTaskChequeTreatment.InsertChequeTreatment();

                        IUpdateTaskChequeInfo iUpdateTaskChequeInfo = new DUpdateTaskChequeInfo(item.ChequeInfoId);
                        iUpdateTaskChequeInfo.UpdateChequeInfo(item.Status,item.StatusDate);


                    }


                    transaction.Complete();
                    result.IsSuccess = true;
                    result.Message = "Successfully Inserted";
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