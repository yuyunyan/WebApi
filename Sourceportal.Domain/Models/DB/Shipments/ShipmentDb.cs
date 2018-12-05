using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Shipments
{
    public class ShipmentDb
    {
        public int ShipmentID { get; set; }
        public string ExternalID { get; set; }
        public string ExternalUUID { get; set; }
        public string CarrierName { get; set; }
        public string TrackingNumber { get; set; }
        public string TrackingURL { get; set; }
        public DateTime ShipDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
