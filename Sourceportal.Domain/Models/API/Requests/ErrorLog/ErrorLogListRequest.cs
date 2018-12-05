using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.ErrorLog
{
    [DataContract]
    public class ErrorLogListRequest
    {
        [DataMember(Name = "appId")]
        public int AppId { get; set; }
        [DataMember(Name = "searchString")]
        public string SearchString { get; set; }
        [DataMember(Name = "rowLimit")]
        public int RowLimit { get; set; }
        [DataMember(Name = "rowOffset")]
        public int RowOffset { get; set; }
        [DataMember(Name = "sortBy")]
        public string SortBy { get; set; }
        [DataMember(Name = "descSort")]
        public bool DescSort { get; set; }
        [DataMember(Name = "dateStart")]
        public DateTime DateStart { get; set; }
        [DataMember(Name = "dateEnd")]
        public DateTime DateEnd { get; set; }
    }
}
