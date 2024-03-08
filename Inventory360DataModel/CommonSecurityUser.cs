namespace Inventory360DataModel
{
    public class CommonSecurityUser
    {
        public long SecurityUserId { get; set; }
        public long DepartmentId { get; set; }
        public long LevelId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string EmailAddress { get; set; }
        public string Active { get; set; }
        public string FirstLogin { get; set; }
        public long CompanyId { get; set; }
        public long LocationId { get; set; }
    }
}
