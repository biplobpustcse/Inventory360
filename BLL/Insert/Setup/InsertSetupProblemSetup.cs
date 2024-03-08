using Inventory360DataModel;
using Inventory360DataModel.Setup;
using BLL.Common;
using DAL.DataAccess.Insert.Setup;
using DAL.DataAccess.Select.Configuration;
using DAL.DataAccess.Select.Setup;
using DAL.Interface.Insert.Setup;
using DAL.Interface.Select.Configuration;
using DAL.Interface.Select.Setup;
using System;
using System.Linq;
using System.Transactions;

namespace BLL.Insert.Setup
{
    public class InsertSetupProblemSetup : GenerateDifferentEventPrefix
    {       
        public CommonResult InsertProblemSetup(CommonSetupProblemSetup entity)
        {
            try
            {
                ISelectSetupProblem iSelectSetupProblem = new DSelectSetupProblem(entity.CompanyId);
                // Get all problem
                var problemLists = iSelectSetupProblem.SelectProblemAll();

                // Check problem name for duplicacy
                if (problemLists.Where(x => x.Name.ToLower() == entity.Name
                    && x.Configuration_OperationalEvent.EventName.Equals(entity.EventName)
                    && x.Configuration_OperationalEvent.SubEventName.Equals(entity.SubEventName)).Count() > 0)
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Duplicate problem name."
                    };
                }

                long problemNewId = FindNewIndexOfTable.FindNewIndexForTable(iSelectSetupProblem.SelectProblemWithoutCheckingCompany().OrderBy(o => o.ProblemId).Select(s => s.ProblemId).ToList());
                entity.ProblemId = problemNewId;

                ISelectConfigurationOperationalEvent iSelectConfigurationOperationalEvent = new DSelectConfigurationOperationalEvent();
                // Get operational event id
                entity.OperationalEventId = iSelectConfigurationOperationalEvent.SelectOperationalEventAll()
                    .Where(x => x.EventName == entity.EventName && x.SubEventName == entity.SubEventName)
                    .Select(s => s.OperationalEventId)
                    .FirstOrDefault();

                // Initialize value
                IInsertSetupProblemSetup iInsertSetupProblemSetup = new DInsertSetupProblemSetup(entity);

                // Insert port
                if (iInsertSetupProblemSetup.InsertProblemSetup())
                {
                    return new CommonResult()
                    {
                        IsSuccess = true,
                        Message = "Insert Successful."
                    };
                }
                else
                {
                    return new CommonResult()
                    {
                        IsSuccess = false,
                        Message = "Insert Unsuccessful."
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