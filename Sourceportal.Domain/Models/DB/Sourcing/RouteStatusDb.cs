using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Sourcing
{
    public class RouteStatusDb
    {
        public int RouteStatusID { get; set; }
        public string StatusName { get; set; }
        public bool IsDefault { get; set; }
        public bool IsComplete { get; set; }
        public int? CountQuoteLines { get; set; }
    }
}
