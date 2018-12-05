using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Requests.VendorRfqs
{
    [DataContract]
    public class VendorRfqLineResponsesGetRequest
    {
        [DataMember(Name = "rfqLineId")]
        public int RfqLineId { get; set; }

        [DataMember(Name = "searchString")]
        public string SearchString { get; set; }

        [DataMember(Name = "rowOffset")]
        public int RowOffset { get; set; }

        [DataMember(Name = "rowLimit")]
        public int RowLimit { get; set; }

        [DataMember(Name = "sortBy")]
        public string SortBy { get; set; }

        [DataMember(Name = "descSort")]
        public int DescSort { get; set; }
    }
}
