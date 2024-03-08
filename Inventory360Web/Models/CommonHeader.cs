namespace Inventory360Web.Models
{
    public class CommonHeader
    {
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string CompanyContact { get { return Contact(Phone, Fax); } }
        public string Approved { get; set; }
        public string Status { get { return GetStatus(Approved); } }
        public string ApprovedBy { get; set; }
        public string CancelReason { get; set; }
        public string EntryByName { get; set; }
        public string CurrencyCultureInfo { get; set; }
        private string Contact(string phone, string fax)
        {
            return "Phone : " + phone + (string.IsNullOrEmpty(fax) ? "" : ", Fax No. : " + fax);
        }

        private string GetStatus(string status)
        {
            return string.IsNullOrEmpty(status) ? string.Empty
                : (status.Equals("N") ? "Unapproved"
                : (status.Equals("A") ? "Approved"
                : "Cancelled"));
        }
    }
}