using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Security
{
    [DataContract]
    public class UserObjectSecurityGetResponse : BaseResponse
    {
        [DataMember(Name = "userObjectSecurities")]
        public List<UserObjectSecurity> UserObjectSecurities { get; set; }
    }

    [DataContract]
    public class UserObjectSecurity
    {
        [DataMember(Name = "fieldId")]
        public int? FieldID { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "canEdit")]
        public bool CanEdit { get; set; }
    }
}
