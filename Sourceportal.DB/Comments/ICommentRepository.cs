using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.Domain.Models.API.Requests.Comments;
using Sourceportal.Domain.Models.DB.Comments;

namespace Sourceportal.DB.Comments
{
    public interface ICommentRepository
    {
        List<CommentDb> GetComments(int ObjectID, int ObjectTypeID, string SearchString);
        CommentDb SetComment(SetCommentRequest setCommentRequest);
    }
}
