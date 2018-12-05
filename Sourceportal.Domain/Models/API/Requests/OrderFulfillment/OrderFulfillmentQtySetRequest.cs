using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Requests.OrderFulfillment
{
    [DataContract]
    public class OrderFulfillmentQtySetRequest
    {
        [DataMember(Name = "soLineId")]
        public int SoLineId { get; set; }

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "qty")]
        public int Quantity { get; set; }

        [DataMember(Name = "isDeleted")]
        public bool IsDeleted { get; set; }
    }
}
