using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.QC
{
    [DataContract]
    public class InspectionCheckListsResponse
    {
        [DataMember(Name = "checkLists")]
        public IList<InspectionCheckList> CheckLists;
    }

    [DataContract]
    public class InspectionCheckList
    {
        [DataMember(Name = "id")]
        public int Id;

        [DataMember(Name = "name")]
        public string Name;

        [DataMember(Name = "addedByUser")]
        public bool AddedByUser { get; set; }

        [DataMember(Name = "questions")]
        public IList<InspectionQuestion> Questions;
    }

    [DataContract]
    public class InspectionQuestion
    {
        [DataMember(Name = "id")]
        public int Id;

        [DataMember(Name = "answerId")]
        public int AnswerId;

        [DataMember(Name = "number")]
        public int Number;

        [DataMember(Name = "text")]
        public string Text;

        [DataMember(Name = "subText")]
        public string SubText;

        [DataMember(Name = "answer")]
        public string Answer;

        [DataMember(Name = "answerTypeId")]
        public int AnswerTypeId;

        [DataMember(Name = "qtyFailed")]
        public int QtyFailed;

        [DataMember(Name = "showQtyFailed")]
        public int ShowQtyFailed;

        [DataMember(Name = "comments")]
        public string Comments;

        [DataMember(Name = "inspected")]
        public bool Inspected;
        
        [DataMember(Name = "imageCount")]
        public int ImageCount;

        [DataMember(Name = "canComment")]
        public bool CanComment;

        [DataMember(Name = "completedDate")]
        public string CompletedDate;

        [DataMember(Name = "requiresPicture")]
        public bool RequiresPicture;

    }
}
