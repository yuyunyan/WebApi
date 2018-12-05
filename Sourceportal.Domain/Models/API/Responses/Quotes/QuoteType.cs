using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.Quotes
{
    [DataContract]
    public class QuoteType { 
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
