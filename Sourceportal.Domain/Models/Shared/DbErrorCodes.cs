using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Sourceportal.Domain.Models.Shared
{
    public class DbErrors
    {
        public static Dictionary<int, string> Codes = new Dictionary<int, string>
        {
            {0, "Success"},
            {-1, "Database operation failed"},
            {-2, "Email address already used"}
        };
    }
}
