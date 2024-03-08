using System;
namespace Inventory360DataModel.Setup
{
    public class CommonSetupProblemSetup
    {
        public long ProblemId { get; set; }
        public string EventName { get; set; }
        public string SubEventName { get; set; }
        public long OperationalEventId { get; set; }
        public string Name { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
    }
}