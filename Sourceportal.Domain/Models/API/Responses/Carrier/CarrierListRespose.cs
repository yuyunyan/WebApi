using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Carrier
{
   public class CarrierListRespose
    {
        [DataContract]
        public class CarriersListResponse
        {
            [DataMember(Name = "carriers")]
            public IList<CarrierResponse> Carriers { get; set; }
        }
        [DataContract]
        public class CarrierResponse
        {

            [DataMember(Name = "carrierID")]
            public int CarrierID { get; set; }

            [DataMember(Name = "carrierName")]
            public string CarrierName { get; set; }
        }
    }
}
