using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.DB.Enum
{
    [Flags]
    public enum DocumentType
    {
        Image = 1,
        Spreadsheet = 2,
        Text = 4,
        Other = 8
    }
}
