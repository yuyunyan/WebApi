using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Requests.Items
{
    public class ItemPOsListGetRequest
    {
        [DataMember(Name = "itemId")]
        public int ItemID { get; set; }

        [DataMember(Name = "rowOffset")]
        public int RowOffset { get; set; }

        [DataMember(Name = "rowLimit")]
        public int RowLimit { get; set; }

        [DataMember(Name = "sortBy")]
        public string SortBy { get; set; }

        [DataMember(Name = "descSort")]
        public bool DescSort { get; set; }
    }
}
