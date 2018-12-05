using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Items
{
    [DataContract]
    public class ItemCommodityListResponse : BaseResponse
    {
        [DataMember(Name = "commodities")]
        public IList<ItemCommodityResponse> Commodities { get; set; }
    }

    [DataContract]
    public class ItemCommodityResponse
    {

        [DataMember(Name = "commodityId")]
        public int CommodityID { get; set; }

        [DataMember(Name = "commodityName")]
        public string CommodityName { get; set; }

        [DataMember(Name = "itemGroupId")]
        public int ItemGroupID { get; set; }
        
    }
}
