using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory360DataModel.Task
{
   public class CommonProductWarehouseByLocation
    {
        public bool IsSelected { get; set; }
        public long WareHouseId { get; set; }
        public string WarehouseName { get; set; }
        public decimal StockQuantity { get; set; }
        public decimal DeliveryQuantity { get; set; }
    }
}
