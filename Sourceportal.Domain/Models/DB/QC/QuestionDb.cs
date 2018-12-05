using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.QC
{
   public class QuestionDb
    {
        public int CheckListId { get; set; }
        public int QuestionId { get; set; }
        public int VersionId { get; set; }
        public int AnswerTypeId { get; set; }
        public string AnswerTypeName { get; set; }
        public string QuestionText { get; set; }
        public string QuestionSubText { get; set; }
        public string QuestionHelpText { get; set; }
        public int SortOrder { get; set; }
        public bool ShowQtyFailed { get; set; }
        public bool CanComment { get; set; }
        public bool RequiresPicture { get; set; }
        public bool RequiresSignature { get; set; }
        public bool PrintOnInspectReport { get; set; }
        public bool PrintOnRejectReport { get; set; }
        public bool IsDeleted { get; set; }
        public int TotalRows { get; set; }
    }
}
