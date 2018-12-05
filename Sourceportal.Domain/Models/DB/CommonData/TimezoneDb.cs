using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.CommonData
{
    public class TimezoneDb
    {
        public string name { get;set; }
        public string current_utc_offset { get; set; }
        public string is_currently_dst { get;set; }
    }
}
