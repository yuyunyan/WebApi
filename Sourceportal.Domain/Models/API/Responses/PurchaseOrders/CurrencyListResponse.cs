using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.PurchaseOrders
{
    public class CurrencyListResponse : BaseResponse
    {
        [DataMember(Name = "currencies")]
        public IList<CurrencyResponse> Currencies { get; set; }
    }

    public class CurrencyResponse
    {
        [DataMember(Name = "currencyId")]
        public string CurrencyID { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "externalId")]
        public string ExternalID { get; set; }
    }
}
