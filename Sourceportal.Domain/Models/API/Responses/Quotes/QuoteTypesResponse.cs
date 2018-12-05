using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.Quotes
{
    public class QuoteTypesResponse
    {

        [DataMember(Name = "quoteType")]
        public List<QuoteType> QuoteType;
    }
}
