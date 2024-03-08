namespace Inventory360DataModel.Setup
{
    public class CommonSetupTermsAndConditions
    {
        public long TermsAndConditionsId { get; set; }
        public string EventName { get; set; }
        public string SubEventName { get; set; }
        public long TemplateHeaderId { get; set; }
        public string Header { get; set; }
        public string Detail { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
    }
}