using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Comments
{
    [DataContract]
    public class CommentTypeIdsResponse : BaseResponse
    {
        [DataMember(Name = "commentTypeIds")]
        public List<CommentTypeMap> CommentTypeIds;
    }

    [DataContract]
    public class CommentTypeMap
    {
        [DataMember(Name = "commentTypeId")]
        public int CommentTypeId { get; set; }

        [DataMember(Name = "typeName")]
        public string TypeName { get; set; }
    }
}
