using DAL.DataAccess;
using DAL.DataAccess.Select.Configuration;
using DAL.Interface;
using DAL.Interface.Select.Configuration;
using System;
using System.Linq;

namespace BLL.Common
{
    public class GenerateDifferentEventPrefix
    {
        protected string GenerateDifferentEventPrefixAsNo(string prefix, string numberFormat, DateTime date, long companyId, long locationId)
        {
            try
            {
                string generatedPrefix = prefix;

                ISelectConfigurationFormattingTag iSelectConfigurationFormattingTag = new DSelectConfigurationFormattingTag();
                var lists = iSelectConfigurationFormattingTag.SelectFormattingTagAll()
                    .Select(s => new
                    {
                        s.Id,
                        s.TagName,
                        s.Type,
                        s.DataSource
                    })
                    .ToList();

                string[] formattingItems = numberFormat.Split('#');
                foreach (string item in formattingItems)
                {
                    int id = 0;
                    int.TryParse(item, out id);

                    if (id == 0)
                    {
                        generatedPrefix += item;
                    }
                    else
                    {
                        var selectedList = lists.Where(x => x.Id == id).FirstOrDefault();

                        if (selectedList.Type == "System")
                        {
                            generatedPrefix += (selectedList.TagName == "Year" ? date.Year.ToString()
                                : (selectedList.TagName == "Month" ? date.Month.ToString()
                                : (selectedList.TagName == "Day" ? date.Day.ToString() : string.Empty)));
                        }
                        else if (selectedList.Type == "DataSource")
                        {
                            long idForSqlQuery = selectedList.TagName == "Company" ? companyId
                                 : locationId;

                            IExecuteSQLQuery iExecuteSQLQuery = new DExecuteSQLQuery();
                            var value = iExecuteSQLQuery.ExecuteQuery(selectedList.DataSource + idForSqlQuery);

                            generatedPrefix += value;
                        }
                    }
                }

                return generatedPrefix;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}