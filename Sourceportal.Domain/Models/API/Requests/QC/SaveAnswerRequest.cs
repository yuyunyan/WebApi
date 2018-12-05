using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Requests.QC
{
    [DataContract]
   public class SaveAnswerRequest
    {
        [DataMember(Name = "answerId")]
        public int AnswerId { get; set; }

        [DataMember(Name = "answer")]
        public string Answer { get; set; }

        [DataMember(Name = "qtyFailed")]
        public int QtyFailed { get; set; }

        [DataMember(Name = "comments")]
        public string Comments { get; set; }

        [DataMember(Name = "inspected")]
        public bool Inspected { get; set; }
    }
}
