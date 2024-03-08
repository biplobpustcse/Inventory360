using Inventory360DataModel;
using DAL.DataAccess.Select.Setup;
using DAL.Interface.Select.Setup;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.DropDown
{
    public class DropDownSetupBank
    {
        private List<CommonResultList> SelectAllBankForDropdown(long companyId, string query, string ownBankOrAll)
        {
            try
            {
                // ownBankOrAll == "A" means all bank
                // ownBankOrAll == "N" means not own bank
                // ownBankOrAll == "Y" means only own bank
                ISelectSetupBank iSelectSetupBank = new DSelectSetupBank(companyId);

                return iSelectSetupBank.SelectBankAll()
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.Name.ToLower().Contains(query.ToLower()))
                    .WhereIf(ownBankOrAll.Equals("N"), x => !x.IsOwnBank)
                    .WhereIf(ownBankOrAll.Equals("Y"), x => x.IsOwnBank)
                    .OrderBy(o => o.Name)
                    .Select(s => new CommonResultList
                    {
                        Item = s.Name.ToString(),
                        Value = s.BankId.ToString()
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CommonResultList> SelectAllBankByCompanyIdForDropdown(long companyId, string query)
        {
            try
            {
                return SelectAllBankForDropdown(companyId, query, "A");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CommonResultList> SelectOwnBankByCompanyIdForDropdown(long companyId, string query)
        {
            try
            {
                return SelectAllBankForDropdown(companyId, query, "Y");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}