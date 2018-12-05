using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.BOMs
{
   public class AvailabilityPartDbs
    {
        public int ItemID { get; set; }
        public string Type { get; set; }
        public string WarehouseName { get; set; }
        public string AccountName { get; set; }
        public int Avaliable { get; set; }
        public int OrigQty { get; set; }
        public string Allocations { get; set; }
        public string DateCode { get; set; }
        public decimal Cost { get; set; }
        public string PackagingName { get; set; }
        public string Buyers { get; set; }
        public string ConditionName { get; set; }
    }
}
