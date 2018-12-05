using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Requests.VendorRfqs
{
    [DataContract]
    public class RfqLineResponseSaveRequest
    {
        [DataMember(Name = "sourceId")]
        public int SourceId { get; set; }

        [DataMember(Name = "rfqLineId")]
        public int RfqLineId { get; set; }

        [DataMember(Name = "offerQty")]
        public int OfferQty { get; set; }

        [DataMember(Name = "cost")]
        public decimal Cost { get; set; }

        [DataMember(Name = "dateCode")]
        public string DateCode { get; set; }

        [DataMember(Name = "moq")]
        public int Moq { get; set; }

        [DataMember(Name = "spq")]
        public int Spq { get; set; }
        
        [DataMember(Name = "packagingId")]
        public int PackagingId { get; set; }
        
        [DataMember(Name = "partNumber")]
        public string PartNumber { get; set; }

        [DataMember(Name = "itemId")]
        public int ItemID { get; set; }

        [DataMember(Name = "manufacturer")]
        public string Manufacturer { get; set; }

        [DataMember(Name = "validforHours")]
        public double ValidforHours { get; set; }

        [DataMember(Name = "leadTimeDays")]
        public int LeadTimeDays { get; set; }

        [DataMember(Name = "isNoStock")]
        public bool IsNoStock { get; set; }

        [DataMember(Name = "isIhs")]
        public bool IsIHS { get; set; }

    }
}
