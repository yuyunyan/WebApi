using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Security
{
    [DataContract]
    public class ObjectTypeSecuritiesGetResponse
    {
        [DataMember(Name = "objectTypeSecurities")]
        public List<ObjectTypeSecurity> ObjectTypeSecurities;
    }

    [DataContract]
    public class ObjectTypeSecurity
    {
        [DataMember(Name = "typeSecurityId")]
        public int TypeSecurityID { get; set; }
        [DataMember(Name = "typeDescription")]
        public string TypeDescription { get; set; }
        [DataMember(Name = "objectTypeId")]
        public int ObjectTypeID { get; set; }
        [DataMember(Name = "filterTypeId")]
        public int FilterTypeID { get; set; }
        [DataMember(Name = "filterObjectTypeId")]
        public int FilterObjectTypeID { get; set; }
    }
}
