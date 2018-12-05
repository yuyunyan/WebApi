using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Items
{
    [DataContract]
    public class ItemAvailabilityResponse
    {
        [DataMember(Name = "items")]
        public List<ItemAvailabilityDetails> ItemAvailbility { get; set; }

        [DataMember(Name = "totalRowCount")]
        public int TotalRowCount;
    }

    [DataContract]
    public class ItemAvailabilityDetails
    {
        [DataMember(Name = "sourceId")]
        public int SourceID { get; set; }

        [DataMember(Name = "accountId")]
        public int AccountID { get; set; }

        [DataMember(Name = "accountName")]
        public string AccountName { get; set; }

        [DataMember(Name = "created")]
        public string Created { get; set; }

        [DataMember(Name = "createdBy")]
        public string CreatedBy { get; set; }

        [DataMember(Name = "dateCode")]
        public string DateCode { get; set; }

        [DataMember(Name = "quantity")]
        public int Quantity { get; set; }

        [DataMember(Name = "rating")]
        public int Rating { get; set; }

        [DataMember(Name = "cost")]
        public string Cost { get; set; }

        [DataMember(Name = "rtpQty")]
        public int RtpQty { get; set; }

        [DataMember(Name = "packagingId")]
        public int PackagingID { get; set; }

        [DataMember(Name = "packagingName")]
        public string PackagingName { get; set; }

        [DataMember(Name = "moq")]
        public string MOQ { get; set; }

        [DataMember(Name = "leadTime")]
        public string LeadTime { get; set; }

        [DataMember(Name = "typeName")]
        public string TypeName { get; set; }

        [DataMember(Name = "externalId")]
        public string ExternalID { get; set; }


    }
}
