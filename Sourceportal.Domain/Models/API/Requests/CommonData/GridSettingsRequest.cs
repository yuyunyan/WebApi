using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.CommonData
{
    public class GridSettingsRequest
    {
        public string GridName { get; set; }
        public string ColumnDef { get; set; }
        public string SortDef { get; set; }
        public string FilterDef { get; set; }
    }
}
