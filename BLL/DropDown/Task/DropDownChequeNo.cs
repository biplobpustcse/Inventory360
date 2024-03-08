using Inventory360DataModel;
using DAL.DataAccess.Select.Task;
using DAL.Interface.Select.Task;
using System;
using System.Linq;

namespace BLL.DropDown.Task
{
    public class DropDownChequeNo
    {
        public object SelectChequeNo(string query, long companyId, long locationId)
        {
            try
            {
                DSelectTaskChequeInfo iSelectTaskChequeInfo = new DSelectTaskChequeInfo(companyId);

                return iSelectTaskChequeInfo.SelectChequeInfoAll()
                    .Where(x=>x.ChequeNo.ToLower().Contains(query.ToLower()))
                    .OrderBy(o => o.ChequeNo)
                    .Select(s => new CommonResultList
                    {
                        Item = s.ChequeNo.ToString(),
                        Value = s.ChequeNo.ToString()
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}