using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Accounts
{
    public class AccountDbErrors
    {
        public static Dictionary<int, string> ErrorCodes = new Dictionary<int, string>
        {
            {-1, "Insert account failed."},
            {-2, "Update account failed, check AccountID"},
            {-3, "Insert location Failed"},
            {-4, "Update location Failed, check LocationID"},
            {-5, "Insert contact failed"},
            {-6, "Update contact failed, check ContactID"}
        };
    }
}
