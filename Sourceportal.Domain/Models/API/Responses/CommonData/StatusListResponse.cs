using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.CommonData
{
    [DataContract]
    public class StatusListResponse
    {
        [DataMember(Name= "statusList")]
        public IList<Status> StatusList;
    }

    [DataContract]
    public class Status
    {
        [DataMember(Name = "id")]
        public int Id;

        [DataMember(Name = "name")]
        public string Name;

        [DataMember(Name = "isDefault")]
        public bool IsDefault;
    }
}
