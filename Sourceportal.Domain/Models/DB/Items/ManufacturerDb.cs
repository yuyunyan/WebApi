using Sourceportal.Domain.Models.DB.shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Items
{
    public class ManufacturerDb : BaseDbResult
    {
        public int MfrID { get; set; }
        public string MfrName { get; set; }
        public string Code { get; set; }
        public string MfrURL { get; set; }
    }
}


