using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.Items
{
    public class SetItemDetailsRequest
    {
        public int itemId { get; set; }
        public string mpn { get; set; }
        public int commodityId { get; set; }
        public int manufacturerId { get; set; }
        public int statusId { get; set; }
        public int groupId { get; set; }
        public string description { get; set; }
        public bool eurohs { get; set; }
        public bool cnrohs { get; set; }
        public string eccn { get; set; }
        public string hts { get; set; }
        public string msl { get; set; }
        public float length { get; set; }
        public float width { get; set; }
        public float depth { get; set; }
        public float weight { get; set; }
    }
}
