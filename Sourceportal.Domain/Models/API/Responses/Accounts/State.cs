using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.Accounts
{
    [DataContract]
    public class State
    {
        [DataMember(Name = "stateId")]
        public int Id;

        [DataMember(Name = "name")]
        public string Name;

        [DataMember(Name = "code")]
        public string Code;
    }
}