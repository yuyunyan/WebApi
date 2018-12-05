using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.Shared
{
    public enum ItemStatusEnum
    {
        [Description("Active")]
        Active = 1,
        [Description("Hold")]
        Hold = 2,
        [Description("Closed")]
        Closed = 3,
        [Description("Canceled")]
        Canceled = 4,
    }
}
