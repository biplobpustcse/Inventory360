namespace Inventory360DataModel
{
    public class CommonSecurityLoginCredential
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public long LoginCompanyId { get; set; }
        public string LoginCompanyCode { get; set; }
        public string LoginCompanyName { get; set; }
        public long LoginLocationId { get; set; }
        public string LoginLocationCode { get; set; }
        public string LoginLocationName { get; set; }
        public string DefaultCurrency { get; set; }
        public long LoginUserId { get; set; }
        public string LoginUserName { get; set; }
        public long LoginLevelId { get; set; }
        public string LoginUserLevel { get; set; }
        public long LoginRoleId { get; set; }
        public string LoginUserRole { get; set; }
        public string IsFirstLogin { get; set; }
    }

    public class ChangeSecurityPassword
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}