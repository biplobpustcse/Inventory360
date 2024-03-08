using System;

namespace Inventory360DataModel.Task
{
    public class CommonTaskProductSerial
    {
        public Guid PrimaryId { get; set; }
        public string Serial { get; set; }
        public string AdditionalSerial { get; set; }
    }
}