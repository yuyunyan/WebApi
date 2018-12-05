using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.DB.Enum
{
    [Flags]
    public enum LocationType
    {
        BillTo = 1,
        ShipTo = 2,
        Other = 4,
    }
}
