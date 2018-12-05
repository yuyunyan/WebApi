using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Items
{
    public class ItemDb
    {
        public int ItemID { get; set; }
        public string PartNumber { get; set; }
        public string PartNumberStrip { get; set; }
        public string MfrName { get; set; }
        public int MfrID { get; set; }
        public int CommodityID { get; set; }
        public string CommodityName { get; set; }
        public int ItemGroupID { get; set; }
        public string GroupName { get; set; }
        public string ECCN { get; set; }
        public bool Eurohs { get; set; }
        public bool Cnrohs { get; set; }
        public int? ItemStatusID { get; set; }
        public string Status { get; set; }
        public int TotalRows { get; set; }
        public string PartDescription { get; set; }
        public string HTS { get; set; }
        public string MSL { get; set; }
        public float WeightG { get; set; }
        public float LengthCM { get; set; }
        public float WidthCM { get; set; }
        public float DepthCM { get; set; }
        public string SourceDataID { get; set; }
        public string ExternalID { get; set; }
        public string DatasheetURL { get; set; }

    }
}
