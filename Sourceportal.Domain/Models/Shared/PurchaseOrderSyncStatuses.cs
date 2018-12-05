using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.Shared
{

    public enum PurchaseOrderSyncStatuses
    {
        [Description("Valid")]
        Valid = 1,
        [Description("NewPoNoLines")]
        NewPoNoLines = 2,
        [Description("SyncedPoNoLines")]
        SyncedPoNoLines = 3,
        [Description("Canceled")]
        Canceled = 4,
    }
}
