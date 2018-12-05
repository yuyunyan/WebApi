using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Quotes
{
    [DataContract]
   public class CommodityOptionsResponse
    {
        [DataMember(Name = "commodities")]
        public List<CommodityResponse> Commodity { get; set; }
        
    }

    [DataContract]
    public class CommodityResponse
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
