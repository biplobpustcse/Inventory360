﻿namespace Inventory360DataModel.Setup
{
    public class CommonSetupProductCategory
    {
        public long ProductCategoryId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long ProductGroupId { get; set; }
        public string GroupName { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
    }
}
