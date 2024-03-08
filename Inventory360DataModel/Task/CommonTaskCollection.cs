using System;
using System.Collections.Generic;

namespace Inventory360DataModel.Task
{
    public class CommonTaskCollection
    {
        public Guid CollectionId { get; set; }
        public string CollectionNo { get; set; }
        public DateTime CollectionDate { get; set; }
        public string SelectedCurrency { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal CollectedAmount { get; set; }
        public long CustomerId { get; set; }
        public long SalesPersonId { get; set; }
        public long CollectedBy { get; set; }
        public string MRNo { get; set; }
        public string Remarks { get; set; }
        public long OperationTypeId { get; set; }
        public long OperationalEventId { get; set; }
        public long LocationId { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
        public List<CommonTaskCollectionDetail> CollectionDetailLists { get; set; }
        public List<CommonTaskCollectionMapping> CollectionMappingLists { get; set; }
    }
}