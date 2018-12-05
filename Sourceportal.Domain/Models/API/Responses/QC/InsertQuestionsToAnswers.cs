using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.QC
{
   public class InsertQuestionsToAnswersRequest
    {
        public int ChecklistID { get; set; }
        public int InspectionID { get; set; }
        public int QuestionID { get; set; }
        public int QuestionVersionID { get; set; }
        public int VersionID { get; set; }
    }
}
