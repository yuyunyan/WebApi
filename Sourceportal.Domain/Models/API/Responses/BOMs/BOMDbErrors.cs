using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.BOMs
{
    public class BOMDbErrors
    {
        public static Dictionary<int, string> ErrorCodes = new Dictionary<int, string>
        {
            { 1, "BOM upload failed." },
            {-1, "Layout saving failed. Missing JSON List of XlsDataMap."},
            {-2, "Layout saving failed. Missing AccountID"},
            {-3, "Layout saving failed. Missing UserID"},
            {-4, "ItemList insert or ItemLines insert failed"},
            {-5, "Process Match failed."}
        };
    }
}
