using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.CommonData
{
    [DataContract]
    public class CurrencyResponse
    {
        [DataMember(Name = "currencies")]
        public IList<Currency> Currencies { get; set; }
    }

    [DataContract]
    public class Currency
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "externalId")]
        public string ExternalID { get; set; }
    }
}
