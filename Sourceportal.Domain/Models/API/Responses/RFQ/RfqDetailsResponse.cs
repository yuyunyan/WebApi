using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.RFQ
{
    [DataContract]
    public class RfqDetailsResponse : BaseResponse
    {
        [DataMember(Name = "vendorRfqId")]
        public int VendorRFQID { get; set; }
        [DataMember(Name = "supplierId")]
        public int SupplierId { get; set; }
        [DataMember(Name = "supplierName")]
        public string SupplierName { get; set; }
        [DataMember(Name = "contactId")]
        public int ContactId { get; set; }
        [DataMember(Name = "contactName")]
        public string ContactName { get; set; }
        [DataMember(Name = "statusId")]
        public int StatusId { get; set; }
        [DataMember(Name = "statusName")]
        public string StatusName { get; set; }
        [DataMember(Name = "sentDate")]
        public string SentDate { get; set; }
        [DataMember(Name = "buyer")]
        public string Buyer { get; set; }
        [DataMember(Name = "currencyId")]
        public string CurrencyId { get; set; }

    }
}
