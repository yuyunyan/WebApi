using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.QC
{
   public class InsertChecklistForInspectionRequest
    {
        public int CheckListID { get; set; }
        public int InspectionID { get; set; }
    }
}
