using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Requests
{
    [DataContract]
    public class SearchFilter
    {
        [DataMember(Name = "searchString")]
        public string SearchString { get; set; }

        [DataMember(Name = "fulfilled")]
        public int Fulfilled { get; set; }

        [DataMember(Name = "rowOffset")]
        public int RowOffset { get; set; }

        [DataMember(Name = "rowLimit")]
        public int RowLimit { get; set; }

        [DataMember(Name = "sortCol")]
        public string SortCol { get; set; }

        [DataMember(Name = "descSort")]
        public bool DescSort { get; set; }
        [DataMember(Name = "itemId")]
        public int? ItemID { get; set; }
        [DataMember(Name = "poLineId")]
        public int? PoLineId { get; set; }
        [DataMember(Name = "filterBy")]
        public string FilterBy { get; set; }
        [DataMember(Name = "filterText")]
        public string FilterText { get; set; }
        [DataMember(Name = "includeCompleted")]
        public bool IncludeCompleted { get; set; }
        [DataMember(Name = "includeCanceled")]
        public bool IncludeCanceled { get; set; }
    }
}
