using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.Sourcing
{
    public class SourceCommentUIDRequest
    {
        public int ObjectID { get; set; }
        public int ObjectTypeID { get; set; }
        public int SourceID { get; set; }
    }
}
