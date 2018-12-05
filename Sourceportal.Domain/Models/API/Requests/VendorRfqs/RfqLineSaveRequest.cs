using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Requests.VendorRfqs
{
    [DataContract]
    public class RfqLineSaveRequest
    {
        [DataMember(Name = "vRfqLineId")]
        public int VrfqLineId { get; set; }
        
        [DataMember(Name = "partNumber")]
        public string PartNumber { get; set; }

        [DataMember(Name = "manufacturer")]
        public string Manufacturer { get; set; }

        [DataMember(Name = "commodityId")]
        public int CommodityId { get; set; }
        
        [DataMember(Name = "qty")]
        public int Qty { get; set; }

        [DataMember(Name = "targetCost")]
        public float TargetCost { get; set; }

        [DataMember(Name = "dateCode")]
        public string DateCode { get; set; }

        [DataMember(Name = "packagingId")]
        public int PackagingId { get; set; }
        
        [DataMember(Name = "note")]
        public string Note { get; set; }

        [DataMember(Name = "rfqId")]
        public int VrfqId { get; set; }

        [DataMember(Name = "itemId")]
        public int? ItemId { get; set; }

        [DataMember(Name = "isIHS")]
        public bool IsIHS { get; set; }

    }
}
