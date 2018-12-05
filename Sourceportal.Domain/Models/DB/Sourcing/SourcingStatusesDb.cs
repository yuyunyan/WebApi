using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Sourcing
{
    public class SourcingStatusesDb
    {
        public int StatusID { get; set; }

        public string StatusName { get; set; }

        public int IsDefault { get; set; }
    }
}
