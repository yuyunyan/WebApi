using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.CommonData
{
   public class WarehouseBinDb
    {
        public int WarehouseBinID { get; set; }
        public int WarehouseID { get; set; }
        public string BinName { get; set; }
        public string ExternalID { get; set; }
    }
}
