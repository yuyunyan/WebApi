using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Sourcing
{
    [DataContract]
    public class SetSourceResponse
    {
        [DataMember(Name = "sourceId")]
        public int SourceId { get; set; }
    }
}
