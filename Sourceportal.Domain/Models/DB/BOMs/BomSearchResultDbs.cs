using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.BOMs
{
   public class BomSearchResultDbs
    {
        public int ItemID { get; set; }
        public String PartNumber { get; set; }
        public string PartNumberStrip { get; set; }
        public int MfrID { get; set; }
        public string MfrName { get; set; }
        public int CommodityID { get; set; }
        public string CommodityName { get; set; }
        public int OnOrder { get; set; }
        public int OnHand { get; set; }
        public int Available { get; set; }
        public int Reserved { get; set; }
    }
}
