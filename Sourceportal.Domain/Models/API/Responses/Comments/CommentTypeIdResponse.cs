using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Comments
{
    [DataContract]
    public class CommentTypeIdResponse : BaseResponse
    {
        [DataMember(Name = "commentTypeId")]
        public int CommentTypeId;
    }
}
