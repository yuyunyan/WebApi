using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.Accounts
{
    [DataContract]
    public class SupplierLineCardMatchRequest
    {
        [DataMember(Name = "commodities")]
        public List<int> Commodities { get; set; }
        [DataMember(Name = "mfrs")]
        public List<MfrObject> Mfrs { get; set; }
    }

    [DataContract]
    public class MfrObject
    {
        [DataMember(Name = "mfrName")]
        public string MfrName { get; set; }
    }
}
