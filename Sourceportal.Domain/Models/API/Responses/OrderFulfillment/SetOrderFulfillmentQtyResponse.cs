using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.OrderFulfillment
{
    [DataContract]
    public class SetOrderFulfillmentQtyResponse : BaseResponse
    {
        [DataMember(Name = "soLineId")]
        public int SOLineID { get; set; }

        [DataMember(Name = "id")]
        public int ID { get; set; }

        [DataMember(Name = "qty")]
        public int Qty { get; set; }
    }
}
