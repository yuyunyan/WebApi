using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.CommonData
{
    public class GridSettingsDb
    {
        public string GridName { get; set; }
        public string ColumnDef { get; set; }
        public string FilterDef { get; set; }
        public string SortDef { get; set; }
    }
}
