using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.OrderFulfillment
{
    public class OFDbErrors
    {
        public static Dictionary<int, string> ErrorCodes = new Dictionary<int, string>
        {
            {-1, "Missing UserID"},
            {-2, "Missing SoLineID"},
            {-3, "Missing InventoryID"},
            {-4, "Error in update sales-order & inventory mapping record"},
            {-5, "Error in insert sales-order & inventory mapping record"}
        };
    }
}
