using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.Comments
{
    public class SetCommentRequest
    {
        public int CommentID { get; set; }
        public int ObjectID { get; set; }
        public int ObjectTypeID { get; set; }
        public int CommentTypeID { get; set; }
        public int ReplyToID { get; set; }
        public string Comment { get; set; }
        public int IsDeleted { get; set; }
    }
}
