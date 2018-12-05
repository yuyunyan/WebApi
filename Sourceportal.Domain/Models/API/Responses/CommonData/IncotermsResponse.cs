using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.CommonData
{

    [DataContract]
    public class IncotermsResponse
    {
        [DataMember(Name = "incoterms")]
        public IList<Incoterm> Incoterms { get; set; }
    }

    [DataContract]
    public class Incoterm
    {
        [DataMember(Name = "id")]
        public int IncotermID { get; set; }

        [DataMember(Name = "incotermName")]
        public string IncotermName { get; set; }

        [DataMember(Name = "externalId")]
        public string ExternalID { get; set; }
    }
}
