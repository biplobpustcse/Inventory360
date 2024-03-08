namespace BizTechDataModel.Inventory360Report
{
    public class ReportIncomeStatement
    {
        public string GroupName { get; set; }
        public string GroupCode { get; set; }
        public string Categorization { get; set; }
        public byte CategorizationId { get; set; }
        public string HeadName { get; set; }
        public decimal Amount { get; set; }
    }
}