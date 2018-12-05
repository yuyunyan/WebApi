using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.Sourcing
{
    public class SetSourceStatus
    {
        public int SourceId { get; set; }
        public int ObjectId { get; set; }
        public int ObjectTypeId { get; set; }
        public bool? IsMatch { get; set; }
        public bool IsJoined { get; set; }
        public int? RTPQty { get; set; }
    }
}
