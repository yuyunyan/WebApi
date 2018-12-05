using System;

namespace Sourceportal.Domain.Models.DB.QC
{
    public class InspectionQuestionDb
    {
        public int QuestionId;
        public int AnswerId;
        public int SortOrder;
        public string QuestionText;
        public string QuestionSubText;
        public string Answer;
        public int AnswerTypeId;
        public int QtyFailed;
        public int ShowQtyFailed;
        public bool CanComment;
        public string Note;
        public string CompletedDate;
        public int ImageCount;
        public bool RequiresPicture;
    }
}
