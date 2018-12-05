using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.QC
{
   public class ChecklistQuestionDb
    {
        public int ChecklistID { get; set; }
        public int QuestionID { get; set; }
        public int VersionID { get; set; }

    }
}
