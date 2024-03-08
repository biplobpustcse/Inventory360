namespace Inventory360DataModel.Setup
{
    public class CommonSetupCustomerShortInfo
    {
        public string GroupName { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }
        public long SalesPersonId { get; set; }
        public string SalesPersonName { get; set; }
        public bool IsCashTypeTransaction { get; set; }
        public bool IsWalkIn { get; set; }
    }
}