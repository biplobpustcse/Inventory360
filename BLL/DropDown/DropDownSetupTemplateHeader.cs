using Inventory360DataModel;
using DAL.DataAccess.Select.Setup;
using DAL.Interface.Select.Setup;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.DropDown
{
    public class DropDownSetupTemplateHeader
    {
        private List<CommonResultList> SelectTermsAndConditionsByEventAndSubEvent(long companyId, string query, string eventName, string subEventName)
        {
            List<CommonResultList> initialList = new List<CommonResultList>();
            ISelectSetupTermsAndConditions iSelectSetupTermsAndConditions = new DSelectSetupTermsAndConditions(companyId);

            initialList.Add(new CommonResultList { Item = "N/A", Value = "0", IsSelected = true });

            List<CommonResultList> result = iSelectSetupTermsAndConditions.SelectTermsAndConditionsAll()
                .Where(x => x.Configuration_OperationalEvent.EventName == eventName
                    && x.Configuration_OperationalEvent.SubEventName == subEventName)
                .WhereIf(!string.IsNullOrEmpty(query), x => x.Setup_TemplateHeader.Name.ToLower().Contains(query.ToLower()))
                .Select(s => new CommonResultList
                {
                    Item = s.Setup_TemplateHeader.Name.ToString(),
                    Value = s.TermsAndConditionsId.ToString()
                })
                .OrderBy(o => o.Item)
                .ToList();

            initialList.AddRange(result);

            return initialList;
        }

        public List<CommonResultList> SelectTemplateHeaderByCompanyId(long companyId, string query)
        {
            try
            {
                ISelectSetupTemplateHeader iSelectSetupTemplateHeader = new DSelectSetupTemplateHeader(companyId);

                return iSelectSetupTemplateHeader.SelectTemplateHeaderAll()
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.Name.ToLower().Contains(query.ToLower()))
                    .OrderBy(o => o.Name)
                    .Select(s => new CommonResultList
                    {
                        Item = s.Name.ToString(),
                        Value = s.TemplateHeaderId.ToString()
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CommonResultList> SelectTemplateHeaderForSalesOrderTermsAndConditionsForDropdown(long companyId, string query)
        {
            try
            {
                return SelectTermsAndConditionsByEventAndSubEvent(companyId, query, CommonEnum.OperationalEvent.Sales.ToString(), CommonEnum.OperationalSubEvent.SalesOrder.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CommonResultList> SelectTemplateHeaderForSalesInvoiceTermsAndConditionsForDropdown(long companyId, string query)
        {
            try
            {
                return SelectTermsAndConditionsByEventAndSubEvent(companyId, query, CommonEnum.OperationalEvent.Sales.ToString(), CommonEnum.OperationalSubEvent.Invoice.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CommonResultList> SelectTemplateHeaderForSalesPaymentForDropdown(long companyId, string query)
        {
            try
            {
                return SelectTermsAndConditionsByEventAndSubEvent(companyId, query, CommonEnum.OperationalEvent.Sales.ToString(), CommonEnum.OperationalSubEvent.Payment.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CommonResultList> SelectTemplateHeaderForPurchaseOrderTermsAndConditionsForDropdown(long companyId, string query)
        {
            try
            {
                return SelectTermsAndConditionsByEventAndSubEvent(companyId, query, CommonEnum.OperationalEvent.Purchase.ToString(), CommonEnum.OperationalSubEvent.PurchaseOrder.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CommonResultList> SelectTemplateHeaderForPurchasePaymentForDropdown(long companyId, string query)
        {
            try
            {
                return SelectTermsAndConditionsByEventAndSubEvent(companyId, query, CommonEnum.OperationalEvent.Purchase.ToString(), CommonEnum.OperationalSubEvent.Payment.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}