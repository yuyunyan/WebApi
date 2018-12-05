using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.QC
{
    public class InspectionDbErrors
    {
        public static Dictionary<int, string> ErrorCodes = new Dictionary<int, string>
        {
            //AnswerSet
            {-1, "Answer ID required."},
            {-2, "Update answer failed, check AnswerId."},
            //ConclusionSet
            {-3, "Inspection conclusion inserted failed."},
            {-4, "Inspection conclusion updated failed."}
        };
    }
}
