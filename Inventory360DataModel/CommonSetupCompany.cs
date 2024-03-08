using System;

namespace Inventory360DataModel
{
    public class CommonSetupCompany
    {
        public long CompanyId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string FaxNo { get; set; }
        public DateTime OpeningDate { get; set; }
    }
}
