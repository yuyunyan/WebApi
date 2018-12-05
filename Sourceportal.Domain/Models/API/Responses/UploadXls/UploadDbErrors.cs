using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.UploadXls
{
    public class UploadDbErrors
    {
        public static Dictionary<int, string> ErrorCodes = new Dictionary<int, string>
        {
            // XlsAccountGet
            {-1, "AccountID is missing"},
            {-2, "XlsType is missing"}
        };
    }
}
