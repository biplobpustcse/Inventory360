using System;
using System.Collections.Generic;

namespace Inventory360Web.Models
{
    public class CommonConvertionRatio : CommonHeader
    {
        public string RatioNo { get; set; }
        public DateTime RatioDate { get; set; }   
        public string CurrencyType { get; set; }
        public List<CommonConvertionRatioDetail> ConvertionRatioDetail { get; set; }        
    }

    public class CommonConvertionRatioDetail
    {
        public System.Guid ConvertionDetailId { get; set; }
        public string ProductName { get; set; }
        public string ProductFor { get; set; }        
        public decimal Quantity { get; set; }
        public string ProductCode { get; set; }
        public string UnitType { get; set; }
        public string ProductDimension { get; set; }     
    }
}