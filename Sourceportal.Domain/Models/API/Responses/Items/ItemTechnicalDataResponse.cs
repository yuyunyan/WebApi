using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Items
{
    [DataContract]
    public class ItemTechnicalDataResponse
    {
        [DataMember(Name = "technicalData")]
        public List<TechnicalDataObject> TechnicalData { get; set; }
    }

    [DataContract]
    public class TechnicalDataObject
    {

        [DataMember(Name = "key")]
        public string Key { get; set; }

        [DataMember(Name = "value")]
        public string Value { get; set; }
    }
}
