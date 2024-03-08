using System.Collections.Generic;

namespace Inventory360DataModel.Setup
{
    public class CommonSetupProductSearch
    {
        public string query { get; set; }
        public long groupId { get; set; }
        public long subGroupId { get; set; }
        public long categoryId { get; set; }
        public long brandId { get; set; }
        public string model { get; set; }
        public List<CommonSetupProductSearchDetail> CommonSetupProductSearchDetail { get; set; }
    }
}