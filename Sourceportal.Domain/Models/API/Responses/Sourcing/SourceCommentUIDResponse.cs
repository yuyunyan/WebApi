using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Sourcing
{
    [DataContract]
    public class SourceCommentUIDResponse
    {
        [DataMember(Name = "commentUId")]
        public int CommentUID { get; set; }
    }
}
