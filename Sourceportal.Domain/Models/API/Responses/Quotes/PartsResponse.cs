using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Quotes
{
    [DataContract]
    public class PartsResponse
    {
        [DataMember(Name = "PartsList")]
        public List<PartDetails> PartsListResponse { get; set; }

    }
}
