using System;
namespace Inventory360DataModel.Setup
{
    public class CommonSetupConvertionRatioDetail
    {        
        public System.Guid ConvertionRatioDetailId { get; set; }
        public System.Guid ConvertionRatioId { get; set; }
        public string ProductFor { get; set; }
        public long ProductId { get; set; }
        public Nullable<long> ProductDimensionId { get; set; }
        public long UnitTypeId { get; set; }
        public decimal Quantity { get; set; }
        public string Remarks { get; set; }
    }
}