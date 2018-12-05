using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.Items
{
    public class SetManufacturerRequest
    {
        public string MfrName { get; set; }

        public int Code { get; set; }

        public string MfrUrl { get; set; }

        public int CreatedBy { get; set; }
    }
}
