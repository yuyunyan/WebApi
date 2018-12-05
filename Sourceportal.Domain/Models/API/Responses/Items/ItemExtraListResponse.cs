using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Items
{
    [DataContract]
    public class ItemExtraListResponse: BaseResponse
    {
        [DataMember(Name = "itemExtras")]
        public IList<ItemExtraResponse> ItemExtras { get; set; }
    }


    [DataContract]
    public class ItemExtraResponse
    {
        [DataMember(Name="itemExtraId")]
        public int ItemExtraId { get; set; }

        [DataMember(Name="extraName")]
        public string ExtraName { get; set; }

        [DataMember(Name="extraDescription")]
        public string ExtraDescription { get; set; }
    }
}
