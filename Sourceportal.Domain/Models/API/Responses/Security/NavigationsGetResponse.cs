using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Security
{
    [DataContract]
    public class NavigationsGetResponse : BaseResponse
    {
        [DataMember(Name = "navigations")]
        public List<NavigationResponse> Navigations{ get; set; }
    }

    [DataContract]
    public class NavigationResponse
    {
        [DataMember(Name = "navId")]
        public int NavID { get; set; }
        [DataMember(Name = "parentNavId")]
        public int ParentNavID { get; set; }
        [DataMember(Name = "interface")]
        public string Interface { get; set; }
        [DataMember(Name = "navName")]
        public string NavName { get; set; }
        [DataMember(Name = "icon")]
        public string Icon { get; set; }
        [DataMember(Name = "sortOrder")]
        public int SortOrder { get; set; }
    }
}
