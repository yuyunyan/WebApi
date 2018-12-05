using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.DB.Enum
{
    [Flags]
    public enum PermissionType
    {
        CanChangeAccountCredit = 1,
        CanChangeAccountOwnership =2, 
        CanChangeAccountStatus = 4,
        CanEditAccountField = 8,
        CanMergeAccounts = 16,
        CanViewAccountField = 32,
        Deleted = 64
    }
}
