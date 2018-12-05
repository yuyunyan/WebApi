using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Carrier
{
    [DataContract]
    public class AccountCarrierListResponse
    {
        [DataMember(Name = "accountCarriers")]
        public IList<AccountCarrierResponse> AccountCarriers { get; set; }
    }
    [DataContract]
    public class AccountCarrierResponse
    {
        [DataMember(Name = "accountID")]
        public int AccountID { get; set; }

        [DataMember(Name = "accountNumber")]
        public string AccountNumber { get; set; }

        [DataMember(Name = "carrierID")]
        public int CarrierID { get; set; }

        [DataMember(Name = "carrierName")]
        public string CarrierName { get; set; }

        [DataMember(Name = "isDefault")]
        public bool    IsDefault { get; set; }
    }
}
