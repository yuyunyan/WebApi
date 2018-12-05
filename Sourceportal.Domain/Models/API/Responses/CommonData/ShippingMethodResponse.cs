using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.CommonData
{
    [DataContract]
    public class ShippingMethodResponse
    {
        [DataMember(Name = "shippingMethods")]
        public List<ShippingMethod> ShippingMethods { get; set; }
    }

    [DataContract]
    public class ShippingMethod
    {
        [DataMember(Name = "shippingMethodId")]
        public int ShippingMethodID { get; set; }
        [DataMember(Name = "methodName")]
        public string MethodName { get; set; }
    }
}
