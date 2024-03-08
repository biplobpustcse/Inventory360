namespace BLL.Common
{
    public static class GetApprovedUnApprovedCancelledStatus
    {
        public static string GetStatus(string status)
        {
            return string.IsNullOrEmpty(status) ? string.Empty
                : (status.Equals("N") ? "Unapproved"
                : (status.Equals("A") ? "Approved"
                : "Cancelled"));
        }
    }
}