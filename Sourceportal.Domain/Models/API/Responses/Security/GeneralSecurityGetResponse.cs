using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Security
{
    [DataContract]
    public class GeneralSecurityGetResponse :BaseResponse
    {
        [DataMember(Name = "generalSecurities")]
        public List<GeneralSecurityObject> GeneralSecurityList { get; set; }
    }

    [DataContract]
    public class GeneralSecurityObject
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "id")]
        public int ID { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
