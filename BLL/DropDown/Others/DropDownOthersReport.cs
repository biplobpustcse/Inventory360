using Inventory360DataModel;
using DAL.DataAccess.Select.Others;
using DAL.Interface.Select.Others;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.DropDown.Others
{
    public class DropDownOthersReport
    {
        private List<CommonResultList> SelectReportNameForDropdownFinally(string reportFor)
        {
            try
            {
                List<CommonResultList> initialList = new List<CommonResultList>();
                ISelectOthersReport iSelectOthersReport = new DSelectOthersReport();

                initialList.Add(new CommonResultList { Item = "Select One...", Value = "", IsSelected = true });

                List<CommonResultList> result = iSelectOthersReport.SelectReportNameAll()
                    .Where(x => x.ReportFor.Trim().Equals(reportFor))
                    .OrderBy(o => o.ReportName)
                    .Select(s => new CommonResultList
                    {
                        Item = s.ReportName.Replace("_", " "),
                        Value = s.ReportName
                    })
                    .ToList();

                initialList.AddRange(result);

                return initialList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CommonResultList> SelectSalesAnalysisReportNameForDropdown()
        {
            try
            {
                return SelectReportNameForDropdownFinally("SalesAnalysis");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CommonResultList> SelectComplainReceiveAnalysisReportName()
        {
            try
            {
                return SelectReportNameForDropdownFinally("ComplainReceiveAnalysis");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CommonResultList> SelectCustomerDeliveryAnalysisReportName()
        {
            try
            {
                return SelectReportNameForDropdownFinally("CustomerDeliveryAnalysis");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CommonResultList> SelectReplacementClaimAnalysisReportName()
        {
            try
            {
                return SelectReportNameForDropdownFinally("ReplacementClaimAnalysis");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CommonResultList> SelectReplacementReceiveAnalysisReportName()
        {
            try
            {
                return SelectReportNameForDropdownFinally("ReplacementReceiveAnalysis");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}