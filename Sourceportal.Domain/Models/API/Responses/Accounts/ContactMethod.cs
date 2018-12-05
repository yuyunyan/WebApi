using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.Accounts
{
    public class ContactMethod
    {
        [DataMember(Name = "id")]
        public int Id;

        [DataMember(Name = "name")]
        public string Name;
    }
}