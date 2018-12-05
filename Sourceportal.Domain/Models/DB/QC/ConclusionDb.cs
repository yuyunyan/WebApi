using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.QC
{
    public class ConclusionDb
    {
        public int InventoryID { get; set; }
        public int LotTotal { get; set; }
        public int QtyPassed { get; set; }
        public int QtyFailedTotal { get; set; }
        public string Introduction { get; set; }
        public string TestResults { get; set; }
        public string Conclusion { get; set; }
        public int InspectionQty { get; set; }

    }
}
