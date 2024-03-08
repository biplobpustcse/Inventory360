using System;
using System.Collections.Generic;

namespace Inventory360DataModel
{
    public class CommonAdvanceSearch<T> where T : class
    {
        public IEnumerable<T> stockLists { get; set; }
        public IEnumerable<T> purchaseLists { get; set; }
        public IEnumerable<T> purchaseReturnLists { get; set; }
        public IEnumerable<T> importInLists { get; set; }
        public IEnumerable<T> stockAdjustmentLists { get; set; }
        public IEnumerable<T> salesInvoiceList { get; set; }
        public IEnumerable<T> salesReturnLists { get; set; }
        public IEnumerable<T> stockTransferLists { get; set; }
        public IEnumerable<T> internalStockTransferLists { get; set; }
        public IEnumerable<T> complainReceiveLists { get; set; }
        public IEnumerable<T> replacementClaimLists { get; set; }        
        public IEnumerable<T> replacementReceiveLists { get; set; }
        public IEnumerable<T> customerDeliveryLists { get; set; }
    }    
}
