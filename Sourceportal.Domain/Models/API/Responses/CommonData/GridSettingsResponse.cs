using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.CommonData
{
    [DataContract]
    public class GridSettingsResponse
    {
        [DataMember(Name = "gridName")]
        public string GridName { get; set; }

        [DataMember(Name = "columnDef")]
        public string ColumnDef { get; set; }

        [DataMember(Name = "sortDef")]
        public string SortDef { get; set; }

        [DataMember(Name = "filterDef")]
        public string FilterDef { get; set; }
    }
}
