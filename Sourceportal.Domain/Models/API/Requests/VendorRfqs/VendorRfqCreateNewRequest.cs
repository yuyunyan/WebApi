using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.VendorRfqs
{
    [DataContract]
    public class VendorRfqCreateNewRequest
    {
        [DataMember(Name = "supplierId")]
        public int SupplierId;
        [DataMember(Name = "contactId")]
        public int ContactId;
        [DataMember(Name = "statusId")]
        public int StatusId;
        [DataMember(Name = "currencyId")]
        public string CurrencyId;
        [DataMember(Name = "lines")]
        public List<RfqLineSaveRequest> Lines;
    }
}
