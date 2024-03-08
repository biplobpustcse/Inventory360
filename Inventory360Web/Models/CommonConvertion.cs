using System;
using System.Collections.Generic;

namespace Inventory360Web.Models
{
    public class CommonConvertion : CommonHeader
    {
        public string ConvertionNo { get; set; }
        public DateTime ConvertionDate { get; set; }
        public string ConvertionType { get; set; }      
        public string RatioNo { get; set; }
        public string Remarks { get; set; }        
        public string Location { get; set; }
        public string CurrencyType { get; set; }
        public List<CommonConvertionDetail> ConvertionDetail { get; set; }        
    }

    public class CommonConvertionDetail
    {
        public System.Guid ConvertionDetailId { get; set; }
        public string ProductName { get; set; }
        public string ProductFor { get; set; }        
        public decimal Quantity { get; set; }
        public string ProductCode { get; set; }
        public string UnitType { get; set; }
        public string ProductDimension { get; set; }
        public List<CommonConvertionDetailSerial> ConvertionDetailSerial { get; set; }       
    }
    public class CommonConvertionDetailSerial
    {
        public System.Guid ConvertionDetailId { get; set; }
        public string Serial { get; set; }
        public string AdditionalSerial { get; set; }
    }
}