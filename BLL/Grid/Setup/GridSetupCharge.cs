using Inventory360DataModel;
using DAL.DataAccess.Select.Configuration;
using DAL.DataAccess.Select.Setup;
using DAL.Interface.Select.Configuration;
using DAL.Interface.Select.Setup;
using System;
using System.Linq;

namespace BLL.Grid.Setup
{
    public class GridSetupCharge
    {
        public object GetAllCharge(long companyId)
        {
            try
            {
                ISelectSetupCharge iSelectSetupCharge = new DSelectSetupCharge(companyId);
                var collectionLists = iSelectSetupCharge.SelectAllCharge()
                    .Select(s => new
                    {
                        isSelected = false,
                        s.ChargeId,
                        s.Name,
                        ChargeAmount = 0
                    });
                return collectionLists;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private object GetAllChargeByOperationalEvent(string operationalEvent, long companyId)
        {
            try
            {
                ISelectConfigurationEventWiseCharge iSelectConfigurationEventWiseCharge = new DSelectConfigurationEventWiseCharge(companyId);
                return iSelectConfigurationEventWiseCharge.SelectEventWiseChargeAll()
                    .Where(x => x.EventName.Equals(operationalEvent))
                    .Select(s => new
                    {
                        isSelected = false,
                        s.ChargeEventId,
                        s.ChargeId,
                        s.Setup_Charge.Name,
                        ChargeAmount = 0
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object GetRMAWiseAllCharge(long companyId)
        {
            try
            {
                return GetAllChargeByOperationalEvent(CommonEnum.OperationalEvent.RMA.ToString(), companyId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}