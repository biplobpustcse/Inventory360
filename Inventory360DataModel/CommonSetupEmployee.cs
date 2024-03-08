namespace Inventory360DataModel
{
    public class CommonSetupEmployee
    {
        private string _role;
        private string _nIDNo;
        private string _passportNo;
        private string _bankAccountNo;
        private long? _accountsId;
        private long? _bankId;

        public long EmployeeId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public long DesignationId { get; set; }
        public string DesignationName { get; set; }
        public string ContactNo { get; set; }
        public string Role {
            get { return string.IsNullOrEmpty(_role) ? null : _role; }
            set { _role = value; }
        }
        public string RoleTitle {
            get { return GetRoleTitle(Role); }
        }
        public string Email { get; set; }
        public string NIDNo {
            get { return string.IsNullOrEmpty(_nIDNo) ? null : _nIDNo; }
            set { _nIDNo = value; }
        }
        public string PassportNo {
            get { return string.IsNullOrEmpty(_passportNo) ? null : _passportNo; }
            set { _passportNo = value; }
        }
        public long? AccountsId {
            get { return _accountsId == 0 ? null : _accountsId; }
            set { _accountsId = value; }
        }
        public string AccountsName { get; set; }
        public long? BankId {
            get { return _bankId == 0 ? null : _bankId; }
            set { _bankId = value; }
        }
        public string BankName { get; set; }
        public string BankAccountNo {
            get { return string.IsNullOrEmpty(_bankAccountNo) ? null : _bankAccountNo; }
            set { _bankAccountNo = value; }
        }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }

        private string GetRoleTitle(string role)
        {
            string roleTitle;
            switch (role)
            {
                case "SP":
                    roleTitle = "Sales Person";
                    break;
                default:
                    roleTitle = string.Empty;
                    break;
            }

            return roleTitle;
        }
    }
}