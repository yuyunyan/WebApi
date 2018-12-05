using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.Shared
{
    public enum RegionsEnum
    {
        [Description("Americas")]
        Americas = 1,
        [Description("EMEA")]
        EMEA = 2,
        [Description("APAC")]
        APAC = 3
    }
}
