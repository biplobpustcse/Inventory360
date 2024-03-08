using System;

namespace Inventory360DataModel.Task
{
    public class CommonTaskCollectionDetail
    {
        public Guid CollectionDetailId { get; set; }
        public Guid CollectionId { get; set; }
        public long PaymentModeId { get; set; }
        public decimal Amount { get; set; }
        public long BankId { get; set; }
        public string ChequeNo { get; set; }
        public DateTime ChequeDate { get; set; }
    }
}