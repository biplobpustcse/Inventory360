namespace Inventory360DataModel
{
    public class CommonHeader
    {
        public long CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string GetStatus(string status)
        {
            return string.IsNullOrEmpty(status) ? string.Empty
                : (status.Equals("N") ? "Unapproved"
                : (status.Equals("A") ? "Approved"
                : "Cancelled"));
        }

        public string CompanyContact { get { return Contact(Phone, Fax); } }

        private string Contact(string phone, string fax)
        {
            return "Phone : " + phone + (string.IsNullOrEmpty(fax) ? "" : ", Fax No. : " + fax);
        }
    }
}