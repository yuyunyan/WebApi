using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Security
{
    [DataContract]
    public class FilterObjectListGetResponse
    {
        [DataMember(Name = "filterObjects")]
        public List<FilterObjectResponse> FilterObjects { get; set; }
    }

    [DataContract]
    public class FilterObjectResponse
    {
        [DataMember(Name = "objectId")]
        public int ObjectID { get; set; }
        [DataMember(Name = "objectTypeId")]
        public int ObjectTypeID { get; set; }
        [DataMember(Name = "objectName")]
        public string ObjectName { get; set; }
    }
}
