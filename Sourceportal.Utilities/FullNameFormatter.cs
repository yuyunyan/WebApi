using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Utilities
{
   public class FullNameFormatter
    {
        public static string FormatFullName(string FirstName, string LastName)
        {
            return (string.IsNullOrEmpty(FirstName) ? "" : FirstName.Trim() + " ") +
                   (string.IsNullOrEmpty(LastName) ? "" : LastName.Trim());
        }

    }
}
