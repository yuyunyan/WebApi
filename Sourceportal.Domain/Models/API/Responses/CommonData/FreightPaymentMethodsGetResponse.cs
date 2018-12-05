using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.CommonData
{
    [DataContract]
    public class FreightPaymentMethodsGetResponse
    {
        [DataMember(Name = "freightPaymentMethods")]
        public List<FreightPaymentMethodResponse> FreightPaymentMethods { get; set; }

    }

    [DataContract]
    public class FreightPaymentMethodResponse
    {
        [DataMember(Name = "freightPaymentMethodId")]
        public int FreightPaymentMethodID { get; set; }
        [DataMember(Name = "methodName")]
        public string MethodName { get; set; }
        [DataMember(Name = "externalId")]
        public int? ExternalID { get; set; }

        [DataMember(Name = "useAccountNum")]
        public bool UseAccountNum { get; set; }

    }
}
