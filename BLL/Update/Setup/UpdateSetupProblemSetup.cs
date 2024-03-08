using Inventory360DataModel;
using Inventory360DataModel.Setup;
using BLL.Common;
using DAL.DataAccess.Select.Configuration;
using DAL.DataAccess.Select.Setup;
using DAL.DataAccess.Update.Setup;
using DAL.Interface.Select.Configuration;
using DAL.Interface.Select.Setup;
using DAL.Interface.Update.Setup;
using System;
using System.Linq;

namespace BLL.Update.Setup
{
    public class UpdateSetupProblemSetup : GenerateDifferentEventPrefix
    {
        public CommonResult UpdateProblemSetup(CommonSetupProblemSetup entity)
        {
            try
            {
                ISelectSetupProblem iSelectSetupProblem = new DSelectSetupProblem(entity.CompanyId);
                // Get all problem
                var problemLists = iSelectSetupProblem.SelectProblemAll();

                // Check problem id that is exist or not
                if (problemLists.Where(x => x.ProblemId.Equals(entity.ProblemId)).Count() == 0)
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Can not edit selected problem. Because of invalid Id provided."
                    };
                }

                var listsExcludeSelectedProblem = problemLists
                    .Where(x => x.ProblemId != entity.ProblemId);

                // Check problem name for duplicacy
                if (listsExcludeSelectedProblem.Where(x => x.Name.ToLower() == entity.Name
                    && x.Configuration_OperationalEvent.EventName.Equals(entity.EventName)
                    && x.Configuration_OperationalEvent.SubEventName.Equals(entity.SubEventName)).Count() > 0)
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Duplicate problem name."
                    };
                }

                ISelectConfigurationOperationalEvent iSelectConfigurationOperationalEvent = new DSelectConfigurationOperationalEvent();
                // Get operational event id
                entity.OperationalEventId = iSelectConfigurationOperationalEvent.SelectOperationalEventAll()
                    .Where(x => x.EventName == entity.EventName && x.SubEventName == entity.SubEventName)
                    .Select(s => s.OperationalEventId)
                    .FirstOrDefault();

                IUpdateSetupProblemSetup iUpdateSetupProblemSetup = new DUpdateSetupProblemSetup(entity);
                if (iUpdateSetupProblemSetup.UpdateProblemSetup())
                {
                    return new CommonResult()
                    {
                        IsSuccess = true,
                        Message = "Update Successful."
                    };
                }
                else
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Update Unsuccessful."
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}