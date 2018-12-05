using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Requests.VendorRfqs
{
    [DataContract]
    public class VendorRfqSaveRequest
    {
        [DataMember(Name = "supplierId")]
        public int SupplierId;
        [DataMember(Name = "contactId")]
        public int ContactId;
        [DataMember(Name = "statusId")]
        public int StatusId;
        [DataMember(Name = "currencyId")]
        public string CurrencyId;
        [DataMember(Name = "rfqId")]
        public int RfqId;
        [DataMember(Name = "organizationId")]
        public int OrganizationId { get; set; }
    }
}
