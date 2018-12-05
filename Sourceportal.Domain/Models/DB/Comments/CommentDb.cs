using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Comments
{
    public class CommentDb
    {
        public int CommentID { get; set; }
        public int CommentTypeID { get; set; }
        public int CreatedBy { get; set; }
        public string Created { get; set; }
        public string Comment { get; set; }
        public int ReplyToID { get; set; }
        public string TypeName { get; set; }
        public string AuthorName { get; set; }
        public string ReplyToName { get; set; }
    }
}
