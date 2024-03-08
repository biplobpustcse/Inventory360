using System.Collections.Generic;

namespace Inventory360DataModel
{
    public class CommonRecordInformation<T> where T : class
    {
        public long TotalNumberOfRecords { get; set; }
        public long Start { get; set; }
        public long End { get; set; }
        public long LastPageNo { get; set; }
        public IEnumerable<T> Data { get; set; }
        public object OthersData { get; set; }
    }
}
