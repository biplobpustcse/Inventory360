using System;
using System.Collections.Generic;

namespace Inventory360DataModel.Task
{
    public class CommonTaskConvertionDetailSerial
    {
        public System.Guid ConvertionDetailSerialId { get; set; }
        public System.Guid ConvertionDetailId { get; set; }
        public string Serial { get; set; }
        public string AdditionalSerial { get; set; }
    }
}