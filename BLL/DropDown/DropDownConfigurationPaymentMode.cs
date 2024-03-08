using DAL.DataAccess.Select.Configuration;
using DAL.Interface.Select.Configuration;
using System;
using System.Linq;

namespace BLL.DropDown
{
    public class DropDownConfigurationPaymentMode
    {
        public object PaymentMode()
        {
            try
            {
                ISelectConfigurationPaymentMode iSelectConfigurationPaymentMode = new DSelectConfigurationPaymentMode();
                return iSelectConfigurationPaymentMode.SelectPaymentModeAll()
                    .OrderBy(o => o.Name)
                    .Select(s => new
                    {
                        Item = s.Name.ToString(),
                        Value = s.PaymentModeId.ToString(),
                        s.NeedDetail
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