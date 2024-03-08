namespace Inventory360DataModel
{
    public  class UserIdentityInfo
    {
        public long CompanyId { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public long LocationId { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string UserLevel { get; set; }
        public string UserRole { get; set; }
        public string DefaultCurrency { get; set; }
    }
}
