using System;
using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.Accounts
{
    [DataContract]
    public class FocusTypeResponse
    {
        [DataMember(Name = "focusTypeId")]
        public int FocusTypeId { get; set; }
        [DataMember(Name = "typeRank")]
        public int TypeRank { get; set; }
        [DataMember(Name = "focusName")]
        public string FocusName { get; set; }
    }
}
