using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.CommonData
{
    [DataContract]
    public class CarrierMethodResponse
    {
        [DataMember(Name = "carrierMethods")]
        public IList<CarrierMethod> CarrierMethods { get; set; }
    }

    [DataContract]
    public class CarrierMethod
    {
        [DataMember(Name = "methodId")]
        public int MethodId { get; set; }

        [DataMember(Name = "methodName")]
        public string MethodName { get; set; }

        [DataMember(Name = "carrierId")]
        public int CarrierId { get; set; }
    }
}
