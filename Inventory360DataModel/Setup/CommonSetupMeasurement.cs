using System.Collections.Generic;

namespace Inventory360DataModel.Setup
{
    public class CommonSetupMeasurement
    {
        public long MeasurementId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
        public List<MeasurementNames> MeasurementNamesList { get; set; }
    }

    public class MeasurementNames {
        public string Name { get; set; }
        public decimal Value { get; set; }
    }
}