namespace Inventory360DataModel
{
    public class CommonEnum
    {
        public enum AdjustmentNature
        {
            A,      // Addition
            D       // Deduction
        }

        public enum AccountsTree
        {
            Group,
            SubGroup,
            Subsidiary,
            Control,
            Accounts
        }

        public enum BalanceType
        {
            Dr,
            Cr
        }

        public enum YesNo
        {
            Yes,
            No
        }

        public enum CashBank
        {
            Cash,
            Bank
        }

        public enum VoucherTypes
        {
            Debit,
            Credit,
            Contra,
            Journal
        }

        public enum EmployeeRole
        {
            SP
        }

        public enum CustomerType
        {
            B,      // Buyer
            C       // Customer
        }

        public enum ProductType
        {
            Asset,
            Consumable,
            Inventory,
            Service
        }

        public enum StockType
        {
            Finished,
            Raw,
            WIP
        }
        public enum ProductStockType
        {
            Current,
            RMA,
            BAD
        }
        public enum RelatedProductType
        {
            RP,     // Related
            SP      // Spare
        }

        public enum OperationalEvent
        {
            Sales,
            Purchase,
            Transfer,
            RMA,
            Production
        }

        public enum OperationType
        {
            Regular
        }

        public enum OperationalSubEvent
        {
            SalesOrder,
            Challan,
            Invoice,
            Collection,
            ItemRequisition,
            RequisitionFinalize,
            TransferOrder,
            PurchaseOrder,
            GoodsReceive,
            ReceiveFinalize,
            Payment,
            ComplainReceive,
            CustomerDelivery,
            ReplacementClaim,
            ReplacementReceive,
            Convertion
        }

        public enum SalesShipmentType
        {
            F,      // Full
            P       // Part
        }

        public enum ShipmentMode
        {
            Air,
            Road,
            Sea
        }

        public enum SalesType
        {
            Regular,
            Installment
        }

        public enum ReportType
        {
            Summary,
            Detail
        }

        public enum IncreaseDecrease
        {
            I,      // Increase (+)
            D       // Decrease (-)
        }

        public enum SalesCollectionAgainst
        {
            Adv,    // Advance
            Pre,    // Previous
            SO,     // Sales Order
            Inv     // Invoice/Bill
        }

        public enum PurchasePaymentAgainst
        {
            Adv,    // Advance
            Pre,    // Previous
            PO,     // Purchase Order
            Bill    // Purchase Receive Finalize
        }

        public enum PurchaseType
        {
            Local,
            Foreign
        }

        // termporary basis for Orix
        public enum MeasurementName
        {
            Length,
            Width,
            Height
        }
    }
}
