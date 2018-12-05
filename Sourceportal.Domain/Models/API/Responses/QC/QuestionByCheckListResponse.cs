using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.QC
{
    [DataContract]
    public class QuestionByCheckListResponse
    {
        [DataMember(Name = "questions")]
        public List<QuestionResponse> QuestionsResponse { get; set; }
           
    }

    [DataContract]
    public class QuestionResponse
    {
        [DataMember(Name = "checkListId")]
        public int CheckListId { get; set; }

        [DataMember(Name = "questionId")]
        public int QuestionId { get; set; }

        [DataMember(Name = "versionId")]
        public int VersionId { get; set; }

        [DataMember(Name = "answerTypeId")]
        public int AnswerTypeId { get; set; }

        [DataMember(Name = "answerTypeName")]
        public string AnswerTypeName { get; set; }

        [DataMember(Name = "questionText")]
        public string QuestionText { get; set; }

        [DataMember(Name = "questionSubText")]
        public string QuestionSubText { get; set; }

        [DataMember(Name = "questionHelpText")]
        public string QuestionHelpText { get; set; }

        [DataMember(Name = "sortOrder")]
        public int SortOrder { get; set; }

        [DataMember(Name = "showQtyFailed")]
        public bool ShowQtyFailed { get; set; }

        [DataMember(Name = "canComment")]
        public bool CanComment { get; set; }

        [DataMember(Name = "requiresPicture")]
        public bool RequiresPicture { get; set; }

        [DataMember(Name = "requireSignature")]
        public bool RequireSignature { get; set; }

        [DataMember(Name = "printOnInspectReport")]
        public bool PrintOnInspectReport { get; set; }

        [DataMember(Name = "printOnRejectReport")]
        public bool PrintOnRejectReport { get; set; }

        [DataMember(Name = "isDeleted")]
        public bool IsDeleted { get; set; }

        [DataMember(Name = "totalRows")]
        public int TotalRows { get; set; }
    }
}
