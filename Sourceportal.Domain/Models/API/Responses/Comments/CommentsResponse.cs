using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Comments
{
    [DataContract]
    public class CommentsResponse
    {
        [DataMember(Name = "comments")]
        public List<CommentResponse> Comments { get; set; } 
    }

    [DataContract]
    public class CommentResponse
    {
        [DataMember(Name = "commentId")]
        public int CommentID { get; set; }

        [DataMember(Name = "commentTypeId")]
        public int CommentTypeID { get; set; }

        [DataMember(Name = "createdBy")]
        public int CreatedBy { get; set; }

        [DataMember(Name = "created")]
        public string Created { get; set; }

        [DataMember(Name = "comment")]
        public string Comment { get; set; }

        [DataMember(Name = "replyToId")]
        public int ReplyToID { get; set; }

        [DataMember(Name = "typeName")]
        public string TypeName { get; set; }

        [DataMember(Name = "authorName")]
        public string AuthorName { get; set; }

        [DataMember(Name = "replyToName")]
        public string ReplyToName { get; set; }
    }
}
