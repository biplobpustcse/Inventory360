using System;
using System.Collections.Generic;

namespace Inventory360DataModel.Task
{
    public class CommonStock_RMAStockSerial
    {
        public System.Guid RMAStockSerialId { get; set; }
        public System.Guid RMAStockId { get; set; }
        public string Serial { get; set; }
        public string AdditionalSerial { get; set; }

    }
}