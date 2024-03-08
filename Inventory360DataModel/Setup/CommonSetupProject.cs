using System;

namespace Inventory360DataModel.Setup
{
    public class CommonSetupProject
    {
        public long ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string IsActive { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
    }
}