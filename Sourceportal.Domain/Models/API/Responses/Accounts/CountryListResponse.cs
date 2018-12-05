using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.Accounts
{
    [DataContract]
    public class CountryListResponse
    {
        [DataMember(Name = "countryList")]
        public List<Country> Countries;
    }
}
