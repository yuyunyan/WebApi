using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.Quotes
{
    [DataContract]
    public class SetQuoteExistingCustomerRequest
    {
        [DataMember(Name = "accountId")]
        public int AccountID { get; set; }

        [DataMember(Name = "contactId")]
        public int ContactID { get; set; }

        [DataMember(Name = "comment")]
        public string Comment { get; set; }

        [DataMember(Name = "quoteParts")]
        public List<SetPartsListRequest> QuoteParts { get; set; }
    }
}
