using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Items
{
    public class ItemDbErrors
    {
        public static Dictionary<int, string> ErrorCodes = new Dictionary<int, string>
        {
            //Item Set
            {-1, "Missing UserID"},
            {-2, "Missing PartNumber"},
            {-3, "Missing CommodityID"},
            {-4, "Missing StatusID"},
            {-5, "Error in updating item"},
            {-6, "Error in inserting item"}
        };
    }
}
