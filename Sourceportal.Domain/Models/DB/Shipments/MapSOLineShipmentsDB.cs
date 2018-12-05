using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Shipments
{
    public class MapSOLineShipmentsDB
    {
        public int SOLineID { get; set; }
        public int ShipmentID { get; set; }
        public bool IsDeleted { get; set; }
        public int Qty { get; set; }
    }
}
