using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Items
{
    public class CommodityDb
    {
        public int CommodityID { get; set; }
        public string CommodityName { get; set; }
        public int ItemGroupID { get; set; }
        public string ExternalID { get; set; }
    }
}
