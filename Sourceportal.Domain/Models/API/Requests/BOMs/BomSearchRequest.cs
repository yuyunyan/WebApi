using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Requests.BOMs
{
    [DataContract]
    public class BomSearchRequest : SearchFilter
    {
        [DataMember(Name = "searchId")]
        public string SearchId { get; set; }

    }
}
