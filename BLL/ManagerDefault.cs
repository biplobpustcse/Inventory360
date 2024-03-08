using Inventory360DataModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class ManagerDefault
    {
        #region Select
        public List<CommonResultList> SelectPartyAdjustmentNature()
        {
            try
            {
                return new CommonList().AdjustmentNature();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CommonResultList> SelectBalanceType()
        {
            List<CommonResultList> lists = new List<CommonResultList>();
            try
            {
                foreach (var item in Enum.GetValues(typeof(CommonEnum.BalanceType)))
                {
                    CommonResultList ii = new CommonResultList();
                    ii.Item = item.ToString();
                    ii.Value = item.ToString();

                    lists.Add(ii);
                }

                return lists;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CommonResultList> SelectYesNo()
        {
            List<CommonResultList> lists = new List<CommonResultList>();
            try
            {
                foreach (var item in Enum.GetValues(typeof(CommonEnum.YesNo)))
                {
                    CommonResultList ii = new CommonResultList();
                    ii.Item = item.ToString();
                    ii.Value = (CommonEnum.YesNo.Yes.ToString() == item.ToString() ? "Y" : "N");

                    lists.Add(ii);
                }

                return lists;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CommonResultList> SelectVoucherTypes()
        {
            try
            {
                return new CommonList().VoucherTypes();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object SelectStockType()
        {
            try
            {
                return new CommonList().SelectStockType();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CommonResultList> AccountsHierarchyTree()
        {
            try
            {
                return new CommonList().AccountsHierarchyTree();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object TrialBalanceType()
        {
            try
            {
                return new CommonList().TrialBalanceType();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CommonResultList> TrialBalanceTree()
        {
            try
            {
                return new CommonList().TrialBalanceTree();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CommonResultList> ReportType()
        {
            try
            {
                return new CommonList().ReportType();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CommonResultList> SelectDebitVoucherType()
        {
            try
            {
                return new CommonList().DebitVoucherType();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CommonResultList> SelectCreditVoucherType()
        {
            try
            {
                return new CommonList().CreditVoucherType();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CommonResultList> SelectCustomerCollectionType()
        {
            try
            {
                return new CommonList().CustomerCollectionType();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CommonResultList> SelectContraVoucherType()
        {
            try
            {
                return new CommonList().ContraVoucherType();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CommonResultList> SelectEmployeeRole()
        {
            try
            {
                return new CommonList().EmployeeRole();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CommonResultList> SelectCustomerType()
        {
            try
            {
                return new CommonList().CustomerType();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CommonResultList> SelectProductType()
        {
            try
            {
                return new CommonList().ProductType();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CommonResultList> SalesShipmentType()
        {
            try
            {
                return new CommonList().SalesShipmentType();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CommonResultList> ShipmentMode()
        {
            try
            {
                return new CommonList().ShipmentMode();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CommonResultList> SalesType()
        {
            try
            {
                return new CommonList().SalesType();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CommonResultList> IncreaseDecrease()
        {
            try
            {
                return new CommonList().IncreaseDecrease();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CommonResultList> SalesCollectionAgainst()
        {
            try
            {
                return new CommonList().SalesCollectionAgainst();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CommonResultList> PurchasePaymentAgainst()
        {
            try
            {
                return new CommonList().PurchasePaymentAgainst();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CommonResultList> PurchaseType()
        {
            try
            {
                return new CommonList().PurchaseType();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object MeasurementNameLists()
        {
            try
            {
                var obj = new List<object> {
                    new { Name = CommonEnum.MeasurementName.Length.ToString(), Value = "0", Unit = "cm" },
                    new { Name = CommonEnum.MeasurementName.Width.ToString(), Value = "0", Unit = "cm" },
                    new { Name = CommonEnum.MeasurementName.Height.ToString(), Value = "0", Unit = "cm" }
                };

                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CommonResultList> SelectChequeType()
        {
            try
            {
                return new CommonList().SelectChequeType();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CommonResultList> SelectChequeStatus()
        {
            try
            {
                return new CommonList().SelectChequeStatus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CommonResultList> SelectReportName()
        {
            try
            {
                return new CommonList().SelectReportName();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CommonResultList> SelectPositionOptionName()
        {
            try
            {
                return new CommonList().SelectPositionOptionName();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        
        public List<CommonChequeStatusGroup> SelectChequeStatusByGroup(string GRP)
        {
            try
            {
                return new CommonList().SelectChequeStatusByGroup().Where(x=>x.GRP == GRP).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object SelectChequeOrTreatementBankOption()
        {
            try
            {
                return new CommonList().SelectChequeOrTreatementBankOption();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object SelectChequeCollectionOrPaymentDateOptionByGroup(string GRP)
        {
            try
            {
                return new CommonList().SelectChequeCollectionOrPaymentDateOptionByGroup().Where(x=>x.GRP == GRP).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public object SelectChequePerformanceType()
        //{
        //    try
        //    {
        //        return new CommonList().SelectChequePerformanceType();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public object SelectProductStockType()
        {
            try
            {
                return new CommonList().ProductStockType();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CommonResultList> SelectRMAExchangeProductOption()
        {
            try
            {
                return new CommonList().SelectRMAExchangeProductOption();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CommonResultList> SelectSpareType()
        {
            try
            {
                return new CommonList().SelectSpareType();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}