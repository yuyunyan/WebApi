using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.Ownership
{
    public class SetOwnershipRequest
    {
        public int ObjectID { get; set; }
        public int ObjectTypeID { get; set; }
        public List<OwnerSetRequest> OwnerList { get; set; }
    }

    public class OwnerSetRequest
    {
        public int UserID { get; set; }
        public decimal Percentage { get; set; }
    }
}
