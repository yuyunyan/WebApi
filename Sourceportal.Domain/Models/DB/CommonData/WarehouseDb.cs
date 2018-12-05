using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.CommonData
{
   public class WarehouseDb
    {
        public int WarehouseID { get; set; }
        public string WarehouseName { get; set; }
        public int LocationID { get; set; }
        public string ExternalID { get; set; }
        public int OrganizationID { get; set; }
        public int ShipFromRegionID { get; set; }
    }
}
