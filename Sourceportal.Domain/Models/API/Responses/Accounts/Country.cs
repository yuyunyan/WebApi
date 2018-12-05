using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.Accounts
{
    [DataContract]
    public class Country
    {
        [DataMember(Name = "countryId")]
        public int Id;

        [DataMember(Name = "name")]
        public string Name;

        [DataMember(Name = "code")]
        public string Code;

        [DataMember(Name = "codeForSap")]
        public string CodeForSap { get; set; }
    }
}