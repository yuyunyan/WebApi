using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.OrderFulfillment
{
    public class OFPoAllocationErrors
    {
        public static Dictionary<int, string> ErrorCodes = new Dictionary<int, string>
        {
            {-1, "Missing SoLineID or PoLineID"},
            {-2, "Missing UserID"}
        };
    }
}
