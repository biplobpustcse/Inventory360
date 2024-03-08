using System;

namespace Inventory360DataModel.Task
{
    public class CommonTaskCollectionMapping
    {
        public Guid CollectionId { get; set; }
        public Guid? SalesOrderId { get; set; }
        public Guid? InvoiceId { get; set; }
        public decimal Amount { get; set; }
    }
}