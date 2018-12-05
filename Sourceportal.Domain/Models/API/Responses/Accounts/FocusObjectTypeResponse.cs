using System;
using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.Accounts
{
    [DataContract]
    public class FocusObjectTypeResponse
    {
        [DataMember(Name = "focusObjectTypeId")]
        public int FocusObjectTypeId { get; set; }
        [DataMember(Name = "objectTypeId")]
        public int ObjectTypeId { get; set; }
        [DataMember(Name = "objectTypeName")]
        public string ObjectTypeName { get; set; }
    }
}
