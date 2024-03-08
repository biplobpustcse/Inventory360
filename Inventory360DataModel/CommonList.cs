using System;
using System.Collections.Generic;

namespace Inventory360DataModel
{
    public class CommonList
    {
        public List<CommonResultList> AdjustmentNature()
        {
            return new List<CommonResultList> {
                new CommonResultList { Item = "Addition", Value = CommonEnum.AdjustmentNature.A.ToString(), IsSelected = true },
                new CommonResultList { Item = "Deduction", Value = CommonEnum.AdjustmentNature.D.ToString() }
            };
        }

        public List<CommonResultList> ProductType()
        {
            List<CommonResultList> lists = new List<CommonResultList>();

            foreach (var item in Enum.GetValues(typeof(CommonEnum.ProductType)))
            {
                CommonResultList ii = new CommonResultList();
                ii.Item = item.ToString();
                ii.Value = item.ToString();
                ii.IsSelected = (item.ToString().Equals(CommonEnum.ProductType.Inventory.ToString()));

                lists.Add(ii);
            }

            return lists;
        }

        public List<CommonResultList> CustomerType()
        {
            return new List<CommonResultList> {
                new CommonResultList { Item = "Customer", Value = CommonEnum.CustomerType.C.ToString(), IsSelected = true },
                new CommonResultList { Item = "Buyer", Value = CommonEnum.CustomerType.B.ToString() }
            };
        }

        public List<CommonResultList> AccountsHierarchyTree()
        {
            return new List<CommonResultList> {
                new CommonResultList { Item = "Accounts", Value = CommonEnum.AccountsTree.Accounts.ToString(), IsSelected = true },
                new CommonResultList { Item = "Subsidiary", Value = CommonEnum.AccountsTree.Subsidiary.ToString() },
                new CommonResultList { Item = "Control", Value = CommonEnum.AccountsTree.Control.ToString() },
                new CommonResultList { Item = "Sub-Group", Value = CommonEnum.AccountsTree.SubGroup.ToString() },
                new CommonResultList { Item = "Group", Value = CommonEnum.AccountsTree.Group.ToString() }
            };
        }

        public List<CommonResultList> TrialBalanceTree()
        {
            return new List<CommonResultList> {
                new CommonResultList { Item = "Accounts Wise", Value = CommonEnum.AccountsTree.Accounts.ToString(), IsSelected = true },
                new CommonResultList { Item = "Subsidiary Wise", Value = CommonEnum.AccountsTree.Subsidiary.ToString() },
                new CommonResultList { Item = "Control Wise", Value = CommonEnum.AccountsTree.Control.ToString() },
                new CommonResultList { Item = "Sub-Group Wise", Value = CommonEnum.AccountsTree.SubGroup.ToString() }
            };
        }

        public object TrialBalanceType()
        {
            return new List<CommonResultList>
            {
                new CommonResultList { Item = "Trial Balance", Value = "TB", IsSelected = true },
                new CommonResultList { Item = "Transactional Trial Balance", Value = "TTB" },
            };
        }

        public List<CommonResultList> ReportType()
        {
            return new List<CommonResultList>
            {
                new CommonResultList { Item = CommonEnum.ReportType.Summary.ToString(), Value = CommonEnum.ReportType.Summary.ToString(), IsSelected = true },
                new CommonResultList { Item = CommonEnum.ReportType.Detail.ToString(), Value = CommonEnum.ReportType.Detail.ToString() },
            };
        }

        public List<CommonResultList> EmployeeRole()
        {
            return new List<CommonResultList> {
                new CommonResultList { Item = "N/A", Value = string.Empty },
                new CommonResultList { Item = "Sales Person", Value = CommonEnum.EmployeeRole.SP.ToString() }
            };
        }

        public List<CommonResultList> CustomerCollectionType()
        {
            return new List<CommonResultList>
            {
                new CommonResultList { Item = "Cash", Value = "Cash" },
                new CommonResultList { Item = "Credit", Value = "Credit" },
                new CommonResultList { Item = "Both", Value = "Both" }
            };
        }

        public object SelectStockType()
        {
            return new List<CommonResultList>
            {
                new CommonResultList { Item = "Finished Goods", Value = CommonEnum.StockType.Finished.ToString(), IsSelected = true },
                new CommonResultList { Item = "Raw Materials", Value = CommonEnum.StockType.Raw.ToString() },
                new CommonResultList { Item = "Work in Progress", Value = CommonEnum.StockType.WIP.ToString() }
            };
        }

        public List<CommonResultList> VoucherTypes()
        {
            return new List<CommonResultList>
            {
                new CommonResultList { Item = CommonEnum.VoucherTypes.Debit.ToString(), Value = "DV", IsSelected = true },
                new CommonResultList { Item = CommonEnum.VoucherTypes.Credit.ToString(), Value = "CV" },
                new CommonResultList { Item = CommonEnum.VoucherTypes.Contra.ToString(), Value = "CON" },
                new CommonResultList { Item = CommonEnum.VoucherTypes.Journal.ToString(), Value = "JV" }
            };
        }

        public List<CommonResultList> DebitVoucherType()
        {
            return new List<CommonResultList>
            {
                new CommonResultList { Item = "Cash Credit", Value = "CC", IsSelected = true },
                new CommonResultList { Item = "Bank Credit", Value = "BC" }
            };
        }

        public List<CommonResultList> CreditVoucherType()
        {
            return new List<CommonResultList>
            {
                new CommonResultList { Item = "Cash Debit", Value = "CD", IsSelected = true },
                new CommonResultList { Item = "Bank Debit", Value = "BD" }
            };
        }

        public List<CommonResultList> ContraVoucherType()
        {
            return new List<CommonResultList>
            {
                new CommonResultList { Item = "Cash To Bank", Value = "CTB", IsSelected = true },
                new CommonResultList { Item = "Cash To Cash", Value = "CTC" },
                new CommonResultList { Item = "Bank To Bank", Value = "BTB" },
                new CommonResultList { Item = "Bank To Cash", Value = "BTC" }
            };
        }

        public List<CommonResultList> SalesShipmentType()
        {
            return new List<CommonResultList>
            {
                new CommonResultList { Item = "Full", Value = CommonEnum.SalesShipmentType.F.ToString(), IsSelected = true },
                new CommonResultList { Item = "Part", Value = CommonEnum.SalesShipmentType.P.ToString() }
            };
        }

        public List<CommonResultList> ShipmentMode()
        {
            return new List<CommonResultList>
            {
                new CommonResultList { Item = CommonEnum.ShipmentMode.Air.ToString(), Value = CommonEnum.ShipmentMode.Air.ToString() },
                new CommonResultList { Item = CommonEnum.ShipmentMode.Road.ToString(), Value = CommonEnum.ShipmentMode.Road.ToString(), IsSelected = true },
                new CommonResultList { Item = CommonEnum.ShipmentMode.Sea.ToString(), Value = CommonEnum.ShipmentMode.Sea.ToString() }
            };
        }

        public List<CommonResultList> SalesType()
        {
            return new List<CommonResultList>
            {
                new CommonResultList { Item = CommonEnum.SalesType.Installment.ToString(), Value = CommonEnum.SalesType.Installment.ToString() },
                new CommonResultList { Item = CommonEnum.SalesType.Regular.ToString(), Value = CommonEnum.SalesType.Regular.ToString(), IsSelected = true }
            };
        }

        public List<CommonResultList> IncreaseDecrease()
        {
            return new List<CommonResultList>
            {
                new CommonResultList { Item = "Increase", Value = CommonEnum.IncreaseDecrease.I.ToString(), IsSelected = true },
                new CommonResultList { Item = "Decrease", Value = CommonEnum.IncreaseDecrease.D.ToString() }
            };
        }

        public List<CommonResultList> SalesCollectionAgainst()
        {
            return new List<CommonResultList>
            {
                new CommonResultList { Item = "Advance", Value = CommonEnum.SalesCollectionAgainst.Adv.ToString() },
                new CommonResultList { Item = "Previous", Value = CommonEnum.SalesCollectionAgainst.Pre.ToString() },
                new CommonResultList { Item="Sales Order", Value = CommonEnum.SalesCollectionAgainst.SO.ToString() },
                new CommonResultList { Item="Invoice / Bill", Value = CommonEnum.SalesCollectionAgainst.Inv.ToString(), IsSelected = true }
            };
        }

        public List<CommonResultList> PurchasePaymentAgainst()
        {
            return new List<CommonResultList>
            {
                new CommonResultList { Item = "Advance", Value = CommonEnum.PurchasePaymentAgainst.Adv.ToString() },
                new CommonResultList { Item = "Previous", Value = CommonEnum.PurchasePaymentAgainst.Pre.ToString() },
                new CommonResultList { Item="Purchase Order", Value = CommonEnum.PurchasePaymentAgainst.PO.ToString() },
                new CommonResultList { Item="Bill", Value = CommonEnum.PurchasePaymentAgainst.Bill.ToString(), IsSelected = true }
            };
        }

        public List<CommonResultList> PurchaseType()
        {
            return new List<CommonResultList>
            {
                new CommonResultList { Item = CommonEnum.PurchaseType.Local.ToString(), Value = CommonEnum.PurchaseType.Local.ToString(), IsSelected = true },
                new CommonResultList { Item = CommonEnum.PurchaseType.Foreign.ToString(), Value = CommonEnum.PurchaseType.Foreign.ToString() }
            };
        }

        public List<CommonAccountsCategorization> AccountsCategorization()
        {
            return new List<CommonAccountsCategorization> {
                new CommonAccountsCategorization { Id = 1, GRP = "AS", Name = "Long Term Assets/Fixed Assets" },
                new CommonAccountsCategorization { Id = 2, GRP = "AS", Name = "Short Term Assets/Current Assets" },
                new CommonAccountsCategorization { Id = 3, GRP = "LB", Name = "Long Term Liabilities/Fixed Liabilities" },
                new CommonAccountsCategorization { Id = 4, GRP = "LB", Name = "Short Term Liabilities/Short Liabilities" },
                new CommonAccountsCategorization { Id = 5, GRP = "OE", Name = "Capital" },
                new CommonAccountsCategorization { Id = 6, GRP = "OE", Name = "Retained Earnings" },
                new CommonAccountsCategorization { Id = 7, GRP = "EX", Name = "Operating Expenses" },
                new CommonAccountsCategorization { Id = 8, GRP = "EX", Name = "Non-Operating Expenses" },
                new CommonAccountsCategorization { Id = 9, GRP = "EX", Name = "Purchase" },
                new CommonAccountsCategorization { Id = 10, GRP = "RE", Name = "Sales Revenue" },
                new CommonAccountsCategorization { Id = 11, GRP = "RE", Name = "Others Revenue" },
                new CommonAccountsCategorization { Id = 12, GRP = "RE", Name = "Sales Return" },
                new CommonAccountsCategorization { Id = 13, GRP = "AS", Name = "Inventory" },
                new CommonAccountsCategorization { Id = 14, GRP = "EX", Name = "Cost of Sales" }
            };
        }

        public object ProductStockType()
        {
            return new List<CommonResultList>
            {
                new CommonResultList { Item = "Current", Value = CommonEnum.ProductStockType.Current.ToString(), IsSelected = true },
                new CommonResultList { Item = "RMA", Value = CommonEnum.ProductStockType.RMA.ToString() },
                new CommonResultList { Item = "BAD", Value = CommonEnum.ProductStockType.BAD.ToString() }
            };
        }

        public List<CommonResultList> SelectChequeType()
        {
            return new List<CommonResultList>
            {
                new CommonResultList { Item = "Received Ckeque", Value = "receivedCheque", IsSelected = true },
                new CommonResultList { Item = "Issued Ckeque", Value = "issuedCheque" }
            };
        }
        public List<CommonResultList> SelectChequeStatus()
        {
            return new List<CommonResultList>
            {
                new CommonResultList { Item = "Not Treated", Value = "N" },
                new CommonResultList { Item = "Send", Value = "S" },
                new CommonResultList { Item = "Dishonor", Value = "D" },
                new CommonResultList { Item = "Honor", Value = "H" },
                new CommonResultList { Item = "Balance Adjusted", Value = "B" }
            };
        }
        public List<CommonChequeStatusGroup> SelectChequeStatusByGroup()
        {
            return new List<CommonChequeStatusGroup>
            {
                //Not Treated
                new CommonChequeStatusGroup { Item = "Send", Value = "S", GRP="N" },
                //Send
                new CommonChequeStatusGroup { Item = "Dishonor", Value = "D", GRP="S"},
                new CommonChequeStatusGroup { Item = "Honor", Value = "H", GRP="S"},
                //Dishonor
                new CommonChequeStatusGroup { Item = "Send", Value = "S", GRP="D" },
                new CommonChequeStatusGroup { Item = "Balance Adjusted", Value = "B",GRP="D" }                                             
            };
        }
        public List<CommonResultList> SelectReportName()
        {
            return new List<CommonResultList>
            {
                new CommonResultList { Item = "Status Wise Cheque Detail", Value = "StatusWiseChequeDetail",IsSelected = true },
                new CommonResultList { Item = "Cheque In Hand", Value = "ChequeInHand" },
                new CommonResultList { Item = "Advance Cheque Issued", Value = "AdvanceChequeIssued" },
                new CommonResultList { Item = "Cheque History", Value = "ChequeHistory" },
                new CommonResultList { Item = "Customer/Supplier wise Cheque Performance", Value = "CustomerSupplierwiseChequePerformance" }
            };

        }
        public List<CommonResultList> SelectChequeOrTreatementBankOption()
        {
            return new List<CommonResultList>
            {
                new CommonResultList { Item = " Compare with Cheque Bank", Value = "Cheque Bank", IsSelected = true },
                new CommonResultList { Item = " Compare with Treatement Bank", Value = "Treatement Bank" }
            };
        }
        public List<CommonChequeStatusGroup> SelectChequeCollectionOrPaymentDateOptionByGroup()
        {
            return new List<CommonChequeStatusGroup>
            {
                //Received Cheque 
                new CommonChequeStatusGroup { Item = "Compare with Cheque Date", Value = "ChequeDate", GRP="receivedCheque",IsSelected = true},
                new CommonChequeStatusGroup { Item = "Compare with Collection Date", Value = "CollectionDate", GRP="receivedCheque"},
                //Issues Cheque
                new CommonChequeStatusGroup { Item = "Compare with Cheque Date", Value = "ChequeDate", GRP="issuedCheque",IsSelected = true },
                new CommonChequeStatusGroup { Item = "Compare with Payment Date", Value = "PaymentDate", GRP="issuedCheque"}
            };
        }
        public List<CommonResultList> SelectPositionOptionName()
        {
            return new List<CommonResultList>
            {
                new CommonResultList { Item = "Current Position", Value = "CurrentPosition",IsSelected = true },
                new CommonResultList { Item = "Back Dated Position", Value = "BackDatedPosition" }
            };
        }

        public List<CommonResultList> SelectRMAExchangeProductOption()
        {
            return new List<CommonResultList>
            {
                new CommonResultList { Item = "Same Product Same Serial", Value = "1", IsSelected = true },
                new CommonResultList { Item = "Same Product Different Serial", Value = "2" },
                new CommonResultList { Item = "Different Product Different Serial", Value = "3" },
                new CommonResultList { Item = "Cash Back", Value = "4" }
            };
        }

        public List<CommonResultList> SelectSpareType()
        {
            return new List<CommonResultList>
            {
                new CommonResultList { Item = "Replacement", Value = "R", IsSelected = true },
                new CommonResultList { Item = "New", Value = "N" }
            };
        }
    }
}