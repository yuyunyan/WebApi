using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Items
{
    [DataContract]
    public class ItemGroupListResponse : BaseResponse
    {
        [DataMember(Name = "itemGroups")]
        public IList<ItemGroupResponse> ItemGroups { get; set; }
    }

    [DataContract]
    public class ItemGroupResponse
    {

        [DataMember(Name = "itemGroupId")]
        public int ItemGroupID { get; set; }

        [DataMember(Name = "groupName")]
        public string GroupName { get; set; }

        [DataMember(Name = "code")]
        public string Code { get; set; }
        
    }
}
